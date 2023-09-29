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
            var ue = mensagemRabbit.ObterObjetoMensagem<UeParaSincronizacaoInstitucionalDto>();
            
            if (ue == null)
                throw new NegocioException("Não foi possível localizar a Ue para sincronizar as turmas.");
            
            var anoAtual = DateTime.Now.Year;

            for (var anoLetivo = (int)ParametrosSistema.AnoInicioSerapEstudantes; anoLetivo <= anoAtual; anoLetivo++)
            {
                var todasTurmasSgp = new List<TurmaSgpDto>();
                
                if (anoLetivo < anoAtual)
                {
                    var turmasSgpHistoricas = (await mediator.Send(new ObterTurmasSgpPorUeCodigoEAnoLetivoQuery(ue.UeCodigo, anoLetivo, true))).ToList();
                    
                    if (turmasSgpHistoricas.Any())
                        todasTurmasSgp.AddRange(turmasSgpHistoricas);
                }                
                
                var turmasSgpNaoHistoricas = (await mediator.Send(new ObterTurmasSgpPorUeCodigoEAnoLetivoQuery(ue.UeCodigo, anoLetivo, false))).ToList();

                if (turmasSgpNaoHistoricas.Any())
                    todasTurmasSgp.AddRange(turmasSgpNaoHistoricas);

                if (!todasTurmasSgp.Any())
                {
                    servicoLog.Registrar(LogNivel.Critico, $"Dre: {ue.DreCodigo}, Ue: {ue.UeCodigo}, AnoLetivo: {anoLetivo} -- Não foi possível localizar as Turmas no Sgp para a sincronização instituicional.");
                    continue;
                }
                
                var todasTurmasSerap = (await mediator.Send(new ObterTurmasSerapPorUeCodigoEAnoLetivoQuery(ue.UeCodigo, anoLetivo))).ToList();

                await TratarInclusao(todasTurmasSgp, todasTurmasSerap, ue.Id);
                await TratarAlteracao(todasTurmasSgp, todasTurmasSerap);
            }

            // -> Sincroniza somente alunos das turmas do ano letivo atual
            await Tratar(ue, anoAtual);

            return true;
        }

        private async Task TratarInclusao(IList<TurmaSgpDto> todasTurmasSgp, IEnumerable<TurmaSgpDto> todasTurmasSerap, long ueId)
        {
            var todasTurmasSgpCodigo = todasTurmasSgp.Select(c => c.Codigo).Distinct();
            var todasTurmasSerapCodigo = todasTurmasSerap.Select(c => c.Codigo).Distinct();
            
            var turmasNovasCodigos = todasTurmasSgpCodigo.Where(a => !todasTurmasSerapCodigo.Contains(a)).ToList();

            if (turmasNovasCodigos.Any())
            {
                var turmasNovasParaIncluir = todasTurmasSgp
                    .Where(a => turmasNovasCodigos.Contains(a.Codigo)).ToList();

                var turmasNovasParaIncluirNormalizada = turmasNovasParaIncluir.Select(a => new Turma
                {
                    Ano = a.NomeTurma.StartsWith("S") ? "S" : a.Ano,
                    AnoLetivo = a.AnoLetivo,
                    Codigo = a.Codigo,
                    ModalidadeCodigo = a.ModalidadeCodigo,
                    NomeTurma = a.NomeTurma,
                    TipoTurma = a.TipoTurma,
                    TipoTurno = a.TipoTurno,
                    UeId = ueId,
                    Semestre = a.Semestre,
                    EtapaEja = a.EtapaEja,
                    SerieEnsino = a.SerieEnsino
                }).ToList();

                await mediator.Send(new InserirTurmasCommand(turmasNovasParaIncluirNormalizada));
            }
        }

        private async Task TratarAlteracao(IList<TurmaSgpDto> todasTurmasSgp, IList<TurmaSgpDto> todasTurmasSerap)
        {
            var todasTurmasSgpCodigo = todasTurmasSgp.Select(c => c.Codigo).Distinct();
            var todasTurmasSerapCodigo = todasTurmasSerap.Select(c => c.Codigo).Distinct();
            
            var turmasParaAlterarCodigos = todasTurmasSgpCodigo.Where(a => todasTurmasSerapCodigo.Contains(a)).ToList();

            if (turmasParaAlterarCodigos.Any())
            {
                var turmasQuePodemAlterar = todasTurmasSgp.Where(a => turmasParaAlterarCodigos.Contains(a.Codigo)).ToList();
                var listaParaAlterar = new List<Turma>();

                foreach (var turmaQuePodeAlterar in turmasQuePodemAlterar)
                {
                    var turmaAntiga = todasTurmasSerap.FirstOrDefault(a => a.Codigo == turmaQuePodeAlterar.Codigo);
                    
                    if (turmaAntiga != null && turmaAntiga.DeveAtualizar(turmaQuePodeAlterar))
                    {
                        listaParaAlterar.Add(new Turma
                        {
                            Ano = turmaQuePodeAlterar.NomeTurma.StartsWith("S") ? "S" : turmaQuePodeAlterar.Ano,
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

        private async Task Tratar(UeParaSincronizacaoInstitucionalDto ue, int anoLetivo)
        {
            var todasTurmasSerap = (await mediator.Send(new ObterTurmasSerapPorUeCodigoEAnoLetivoQuery(ue.UeCodigo, anoLetivo))).ToList();

            var turmasParaSincronizacaoInstitucional = todasTurmasSerap.Select(turma =>
                new TurmaParaSincronizacaoInstitucionalDto(turma.Id, turma.AnoLetivo, turma.Codigo,
                    turma.ModalidadeCodigo, turma.Semestre, ue.Id, ue.UeCodigo)).ToList();
            
            await mediator.Send(new PublicaFilaRabbitCommand(
                RotasRabbit.SincronizaEstruturaInstitucionalTurmaTratar,
                turmasParaSincronizacaoInstitucional));            
        }
    }
}
