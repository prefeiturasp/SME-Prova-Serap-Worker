using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExecutarSincronizacaoInstitucionalAlunoTratarUseCase : AbstractUseCase, IExecutarSincronizacaoInstitucionalAlunoTratarUseCase
    {
        public ExecutarSincronizacaoInstitucionalAlunoTratarUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var turma = mensagemRabbit.ObterObjetoMensagem<TurmaSgpDto>();

            if (turma == null)
                throw new NegocioException($"Turma não informada.");

            var alunosAtivosEol = await mediator.Send(new ObterAlunosEolPorTurmasCodigoQuery(new long[] { long.Parse(turma.Codigo) }));

            if (alunosAtivosEol != null && alunosAtivosEol.Any())
            {
                var alunosEolAgrupadosParaTratarCodigos = alunosAtivosEol.Select(a => a.CodigoAluno).Distinct().ToList();
                var alunosSerap = await mediator.Send(new ObterAlunosSerapPorCodigosQuery(alunosEolAgrupadosParaTratarCodigos.ToArray()));

                List<long> alunosSerapCodigo = new List<long>();
                if (alunosSerap != null && alunosSerap.Any())
                    alunosSerapCodigo = alunosSerap.Select(a => a.RA).Distinct().ToList();

                await TratarInclusao(alunosAtivosEol, alunosEolAgrupadosParaTratarCodigos, alunosSerapCodigo, turma);

                await TratarAlteracao(alunosAtivosEol, alunosEolAgrupadosParaTratarCodigos, alunosSerap, alunosSerapCodigo, turma);

                await TratarInativo(alunosAtivosEol, turma);

                await PublicarSincronizacaoAlunoDeficiencia(alunosEolAgrupadosParaTratarCodigos);
            }

            return true;
        }

        private async Task TratarInclusao(IEnumerable<AlunoEolDto> todasAlunosEol, List<long> todosAlunosEolCodigo, List<long> todosAlunosSerapCodigo, TurmaSgpDto turma)
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
                        TurmaId = turma.Id
                    }).ToList();

                    await mediator.Send(new InserirAlunosCommand(alunosNovosParaIncluirNormalizada));
                }
            }
        }

        private async Task TratarAlteracao(IEnumerable<AlunoEolDto> todasAlunosEol, List<long> todasAlunosEolCodigo, IEnumerable<Aluno> todasAlunosSerap, List<long> todasAlunosSerapCodigo, TurmaSgpDto turma)
        {
            var alunosParaAlterarCodigos = todasAlunosEolCodigo.Where(a => todasAlunosSerapCodigo.Contains(a)).ToList();

            if (alunosParaAlterarCodigos != null && alunosParaAlterarCodigos.Any())
            {
                var alunosQuePodemAlterar = todasAlunosEol.Where(a => alunosParaAlterarCodigos.Contains(a.CodigoAluno)).ToList();
                var listaParaAlterar = new List<Aluno>();

                foreach (var alunoQuePodeAlterar in alunosQuePodemAlterar)
                {
                    var alunoAntigo = todasAlunosSerap.FirstOrDefault(a => a.RA == alunoQuePodeAlterar.CodigoAluno);

                    var turmaFix = await mediator.Send(new ObterTurmaSerapPorIdQuery(alunoAntigo.TurmaId));
                    var turmaAntigaDoAluno = new TurmaSgpDto() { Id = turmaFix.Id, Codigo = turmaFix.Codigo };


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
                            turmaId = turma.Id;

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
                {
                    await mediator.Send(new AlterarAlunosCommand(listaParaAlterar));
                    await mediator.Send(new RemoverAlunosCacheCommand(listaParaAlterar.Select(x => x.RA).ToArray()));
                }
            }
        }

        private async Task TratarInativo(IEnumerable<AlunoEolDto> alunosEol, TurmaSgpDto turma)
        {
            var alunosTurma = await mediator.Send(new ObterAlunosPorTurmaIdQuery(turma.Id));
            if (alunosTurma != null && alunosTurma.Any())
            {
                var alunosInativos = alunosTurma.Where(t => !alunosEol.Any(x => x.CodigoAluno == t.RA)).ToList();
                if (alunosInativos.Any())
                {
                    await mediator.Send(new InativarAlunosCommand(turma.Id, alunosInativos));
                    await mediator.Send(new RemoverAlunosCacheCommand(alunosInativos.Select(x => x.RA).ToArray()));
                }
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
