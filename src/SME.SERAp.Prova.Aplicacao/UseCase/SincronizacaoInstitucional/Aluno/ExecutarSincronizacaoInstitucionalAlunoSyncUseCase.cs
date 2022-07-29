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
    public class ExecutarSincronizacaoInstitucionalAlunoSyncUseCase : AbstractUseCase, IExecutarSincronizacaoInstitucionalAlunoSyncUseCase
    {
       private readonly IServicoLog servicoLog;
        public ExecutarSincronizacaoInstitucionalAlunoSyncUseCase(IMediator mediator, IServicoLog servicoLog) : base(mediator)
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

            var turmasDaDre = await mediator.Send(new ObterTurmasSerapPorDreCodigoQuery(dre.DreCodigo));
            if (turmasDaDre != null && turmasDaDre.Any())
            {
                foreach (var turma in turmasDaDre)
                {
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.SincronizaEstruturaInstitucionalAlunoTratar, turma));
                }
            }
            else throw new NegocioException($"Não foi possível localizar as turmas da Dre {dre.DreCodigo} para fazer sync dos alunos.");

            return true;
        }

        private async Task TratarInclusao(IEnumerable<AlunoEolDto> todasAlunosEol, List<long> todosAlunosEolCodigo, List<long> todosAlunosSerapCodigo, IEnumerable<TurmaSgpDto> turmaSerapDtos)
        {
            var alunosNovasCodigos = todosAlunosEolCodigo.Where(a => !todosAlunosSerapCodigo.Contains(a)).ToList();

            if (alunosNovasCodigos != null && alunosNovasCodigos.Any())
            {
                var alunosNovosParaIncluir = todasAlunosEol.Where(a => alunosNovasCodigos.Contains(a.CodigoAluno)).ToList();
                if (alunosNovosParaIncluir != null && alunosNovosParaIncluir.Any())
                {
                    var alunosNovosParaIncluirNormalizada = alunosNovosParaIncluir.Select(a => new Aluno()
                    {
                        Nome = a.Nome,
                        RA = a.CodigoAluno,
                        Situacao = a.SituacaoAluno,
                        NomeSocial = a.NomeSocial,
                        Sexo = a.Sexo,
                        DataNascimento = a.DataNascimento,
                        TurmaId = turmaSerapDtos.Any(b => b.Codigo == a.TurmaCodigo.ToString()) ? turmaSerapDtos.FirstOrDefault(b => b.Codigo == a.TurmaCodigo.ToString()).Id : 0
                    }).ToList();

                    var alunosSemturma = alunosNovosParaIncluirNormalizada.Where(a => a.TurmaId == 0);
                    if (alunosSemturma.Any())
                        servicoLog.Registrar(LogNivel.Informacao, $"Turma não localizada para os alunos: {string.Join(",", alunosSemturma.Select(a => a.RA.ToString()))}");

                    await mediator.Send(new InserirAlunosCommand(alunosNovosParaIncluirNormalizada.Where(a => a.TurmaId > 0)));
                }
            }
        }
        private async Task TratarAlteracao(IEnumerable<AlunoEolDto> todasAlunosEol, List<long> todasAlunosEolCodigo, IEnumerable<Aluno> todasAlunosSerap, List<long> todasAlunosSerapCodigo, IEnumerable<TurmaSgpDto> turmaSerapDtos)
        {
            var alunosParaAlterarCodigos = todasAlunosEolCodigo.Where(a => todasAlunosSerapCodigo.Contains(a)).ToList();

            if (alunosParaAlterarCodigos != null && alunosParaAlterarCodigos.Any())
            {
                var alunosQuePodemAlterar = todasAlunosEol.Where(a => alunosParaAlterarCodigos.Contains(a.CodigoAluno)).ToList();
                var listaParaAlterar = new List<Aluno>();

                foreach (var alunoQuePodeAlterar in alunosQuePodemAlterar)
                {
                    var alunoAntigo = todasAlunosSerap.FirstOrDefault(a => a.RA == alunoQuePodeAlterar.CodigoAluno);

                    var turmaAntigaDoAluno = turmaSerapDtos.FirstOrDefault(a => a.Id == alunoAntigo.TurmaId);

                    if (turmaAntigaDoAluno == null)
                    {
                        var turmaFix = await mediator.Send(new ObterTurmaSerapPorIdQuery(alunoAntigo.TurmaId));
                        turmaAntigaDoAluno = new TurmaSgpDto() { Id = turmaFix.Id, Codigo = turmaFix.Codigo };
                    }


                    //TODO: Normalizar com uma entidade AlunoEol
                    if (alunoAntigo != null && (alunoAntigo.Nome != alunoQuePodeAlterar.Nome ||
                                                alunoAntigo.Situacao != alunoQuePodeAlterar.SituacaoAluno ||
                                                long.Parse(turmaAntigaDoAluno.Codigo) != alunoQuePodeAlterar.TurmaCodigo ||
                                                alunoAntigo.DataNascimento != alunoQuePodeAlterar.DataNascimento ||
                                                alunoAntigo.NomeSocial != alunoQuePodeAlterar.NomeSocial ||
                                                alunoAntigo.Sexo != alunoQuePodeAlterar.Sexo))
                    {

                        var turmaId = alunoAntigo.TurmaId;
                        if (long.Parse(turmaAntigaDoAluno.Codigo) != alunoQuePodeAlterar.TurmaCodigo)
                        {
                            var turmaCodigoParaBuscar = alunoQuePodeAlterar.TurmaCodigo.ToString();
                            var turmaNova = turmaSerapDtos.FirstOrDefault(a => a.Codigo == turmaCodigoParaBuscar);
                            if (turmaNova == null)
                            {
                                var turmaParaAlunoNovo = await mediator.Send(new ObterTurmaPorCodigoUeQuery(turmaCodigoParaBuscar));
                                if (turmaParaAlunoNovo == null)
                                {
                                   servicoLog.Registrar(LogNivel.Critico, $"Turma não localizada para o aluno {alunoQuePodeAlterar.CodigoAluno}");
                                    continue;
                                }
                                turmaId = turmaParaAlunoNovo.Id;
                            }

                            if (turmaNova != null)
                                turmaId = turmaNova.Id;

                            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.SincronizaEstruturaInstitucionalTurmaAlunoHistoricoTratar, new long[] { alunoQuePodeAlterar.CodigoAluno }));
                        }

                        listaParaAlterar.Add(new Aluno()
                        {
                            Id = alunoAntigo.Id,
                            Nome = alunoQuePodeAlterar.Nome,
                            RA = alunoQuePodeAlterar.CodigoAluno,
                            Situacao = alunoQuePodeAlterar.SituacaoAluno,
                            NomeSocial = alunoQuePodeAlterar.NomeSocial,
                            Sexo = alunoQuePodeAlterar.Sexo,
                            DataAtualizacao = DateTime.Now,
                            DataNascimento = alunoQuePodeAlterar.DataNascimento,
                            TurmaId = turmaId
                        });
                    }
                }

                if (listaParaAlterar.Any())
                    await mediator.Send(new AlterarAlunosCommand(listaParaAlterar));
            }
        }
        private async Task PublicarSincronizacaoAlunoDeficiencia(List<long> alunosRa)
        {
            foreach (long alunoRa in alunosRa)
            {
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TratarAlunoDeficiencia, alunoRa));
            }
        }
    }
}
