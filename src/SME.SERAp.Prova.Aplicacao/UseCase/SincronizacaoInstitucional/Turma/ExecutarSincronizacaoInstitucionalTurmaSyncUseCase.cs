using MediatR;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExecutarSincronizacaoInstitucionalTurmaSyncUseCase : AbstractUseCase, IExecutarSincronizacaoInstitucionalTurmaSyncUseCase
    {
        private readonly IServicoLog servicoLog;
        public ExecutarSincronizacaoInstitucionalTurmaSyncUseCase(IMediator mediator, IServicoLog servicoLog) : base(mediator)
        {
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var dre = mensagemRabbit.ObterObjetoMensagem<DreParaSincronizacaoInstitucionalDto>();

            if (dre == null)
            {
                var mensagem = $"Não foi possível fazer parse da mensagem para sync de turmas da dre {mensagemRabbit.Mensagem}.";
                throw new NegocioException(mensagem);
            }

            long anoAtual = DateTime.Now.Year;

            for (long anoLetivo = (long)ParametrosSistema.AnoInicioSerapEstudantes; anoLetivo <= anoAtual; anoLetivo++)
            {
                var todasTurmasSgp = new List<TurmaSgpDto>();

                if (anoLetivo < anoAtual)
                {
                    var turmasSgpHistoricas = await mediator.Send(new ObterTurmasSgpPorDreCodigoEAnoLetivoQuery(dre.DreCodigo, anoLetivo, true));
                    if (turmasSgpHistoricas != null && turmasSgpHistoricas.Any())
                        todasTurmasSgp.AddRange(turmasSgpHistoricas);
                }

                var turmasSgpNaoHistoricas = await mediator.Send(new ObterTurmasSgpPorDreCodigoEAnoLetivoQuery(dre.DreCodigo, anoLetivo, false));
                if (turmasSgpNaoHistoricas != null && turmasSgpNaoHistoricas.Any())
                    todasTurmasSgp.AddRange(turmasSgpNaoHistoricas);

                var turmasSgp = todasTurmasSgp.AsEnumerable();
                if (turmasSgp == null || !turmasSgp.Any())
                {
                    servicoLog.Registrar(LogNivel.Critico, $"Dre: {dre.DreCodigo}, AnoLetivo: {anoLetivo} -- Não foi possível localizar as Turmas no Sgp para a sincronização instituicional.");
                    continue;
                }

                var turmasSgpCodigo = turmasSgp.Select(a => a.Codigo).Distinct().ToList();

                var turmasSerap = await mediator.Send(new ObterTurmasSerapPorDreCodigoEAnoLetivoQuery(dre.DreCodigo, anoLetivo));
                var turmasSerapCodigo = turmasSerap.Select(a => a.Codigo).Distinct().ToList();

                await TratarInclusao(turmasSgp, turmasSgpCodigo, turmasSerapCodigo, dre.DreCodigo);

                await TratarAlteracao(turmasSgp, turmasSgpCodigo, turmasSerap, turmasSerapCodigo);
            }

            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.SincronizaEstruturaInstitucionalAlunoSync, new DreParaSincronizacaoInstitucionalDto(dre.Id, dre.DreCodigo)));
            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.SincronizaEstruturaInstitucionalAtualizarUeTurma, dre));

            return true;
        }
        private async Task TratarInclusao(IEnumerable<TurmaSgpDto> todasTurmasSgp, List<string> todasTurmasSgpCodigo, List<string> todasTurmasSerapCodigo, string dreCodigo)
        {
            var turmasNovasCodigos = todasTurmasSgpCodigo.Where(a => !todasTurmasSerapCodigo.Contains(a)).ToList();

            if (turmasNovasCodigos != null && turmasNovasCodigos.Any())
            {
                var uesSerap = await mediator.Send(new ObterUesSerapPorDreCodigoQuery(dreCodigo));

                var turmasNovasParaIncluir = todasTurmasSgp.Where(a => turmasNovasCodigos.Contains(a.Codigo)).ToList();

                var turmasNovasParaIncluirNormalizada = turmasNovasParaIncluir.Select(a => new Turma()
                {
                    Ano = a.Ano,
                    AnoLetivo = a.AnoLetivo,
                    Codigo = a.Codigo,
                    ModalidadeCodigo = a.ModalidadeCodigo,
                    NomeTurma = a.NomeTurma,
                    TipoTurma = a.TipoTurma,
                    TipoTurno = a.TipoTurno,
                    UeId = uesSerap.FirstOrDefault(a => a.CodigoUe == a.CodigoUe).Id,
                    Semestre = a.Semestre,
                    EtapaEja = a.EtapaEja,
                    SerieEnsino = a.SerieEnsino
                }).ToList();

                await mediator.Send(new InserirTurmasCommand(turmasNovasParaIncluirNormalizada));
            }
        }
        private async Task TratarAlteracao(IEnumerable<TurmaSgpDto> todasTurmasSgp, List<string> todasTurmasSgpCodigo, IEnumerable<TurmaSgpDto> todasTurmasSerap, List<string> todasTurmasSerapCodigo)
        {
            var turmasParaAlterarCodigos = todasTurmasSgpCodigo.Where(a => todasTurmasSerapCodigo.Contains(a)).ToList();

            if (turmasParaAlterarCodigos != null && turmasParaAlterarCodigos.Any())
            {
                var turmasQuePodemAlterar = todasTurmasSgp.Where(a => turmasParaAlterarCodigos.Contains(a.Codigo)).ToList();
                var listaParaAlterar = new List<Turma>();

                foreach (var turmaQuePodeAlterar in turmasQuePodemAlterar)
                {
                    var turmaAntiga = todasTurmasSerap.FirstOrDefault(a => a.Codigo == turmaQuePodeAlterar.Codigo);
                    if (turmaAntiga != null && turmaAntiga.DeveAtualizar(turmaQuePodeAlterar))
                    {
                        listaParaAlterar.Add(new Turma()
                        {
                            Ano = turmaQuePodeAlterar.Ano,
                            AnoLetivo = turmaQuePodeAlterar.AnoLetivo,
                            Codigo = turmaQuePodeAlterar.Codigo,
                            ModalidadeCodigo = turmaQuePodeAlterar.ModalidadeCodigo,
                            NomeTurma = turmaQuePodeAlterar.NomeTurma,
                            TipoTurma = turmaQuePodeAlterar.TipoTurma,
                            TipoTurno = turmaQuePodeAlterar.TipoTurno,
                            UeId = turmaAntiga.UeId,
                            Id = turmaAntiga.Id,
                            Semestre = turmaQuePodeAlterar.Semestre,
                            EtapaEja = turmaQuePodeAlterar.EtapaEja,
                            SerieEnsino = turmaQuePodeAlterar.SerieEnsino
                        });
                    }
                }

                if (listaParaAlterar.Any())
                    await mediator.Send(new AlterarTurmasCommand(listaParaAlterar));
            }
        }
    }
}
