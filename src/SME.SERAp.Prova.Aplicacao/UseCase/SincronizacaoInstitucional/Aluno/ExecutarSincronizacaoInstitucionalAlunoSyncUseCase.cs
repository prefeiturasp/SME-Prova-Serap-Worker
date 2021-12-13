using MediatR;
using Sentry;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos;
using SME.SERAp.Prova.Infra.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExecutarSincronizacaoInstitucionalAlunoSyncUseCase : AbstractUseCase, IExecutarSincronizacaoInstitucionalAlunoSyncUseCase
    {
        public ExecutarSincronizacaoInstitucionalAlunoSyncUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var dre = mensagemRabbit.ObterObjetoMensagem<DreParaSincronizacaoInstitucionalDto>();

            if (dre == null)
            {
                var mensagem = $"Não foi possível fazer parse da mensagem para sync de turmas da dre {mensagemRabbit.Mensagem}.";
                SentrySdk.CaptureMessage(mensagem);
                throw new NegocioException(mensagem);
            }

            var turmasDaDre = await mediator.Send(new ObterTurmasSerapPorDreCodigoQuery(dre.DreCodigo));
            if (turmasDaDre != null && turmasDaDre.Any())
            {
                var turmasDaDreCodigo = turmasDaDre.Select(a => long.Parse(a.Codigo)).Distinct().ToArray();
                var turmasDaDreId = turmasDaDre.Select(a => a.Id).Distinct().ToArray();
                
                var alunosEol = await mediator.Send(new ObterAlunosEolPorTurmasCodigoQuery(turmasDaDreCodigo));


                for (int i = 0; i < alunosEol.Count(); i += 500)
                {
                    var alunosEolAgrupadosParaTratar = alunosEol.Skip(i).Take(500);
                    var alunosEolAgrupadosParaTratarCodigos = alunosEolAgrupadosParaTratar.Select(a => a.CodigoAluno).Distinct().ToList();

                    var alunosSerap = await mediator.Send(new ObterAlunosSerapPorCodigosQuery(alunosEolAgrupadosParaTratarCodigos.ToArray()));
                    var alunosSerapCodigo = alunosSerap.Select(a => a.RA).Distinct().ToList();

                    await TratarInclusao(alunosEolAgrupadosParaTratar, alunosEolAgrupadosParaTratarCodigos, alunosSerapCodigo, turmasDaDre);

                    await TratarAlteracao(alunosEolAgrupadosParaTratar, alunosEolAgrupadosParaTratarCodigos, alunosSerap, alunosSerapCodigo, turmasDaDre);

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

                var alunosNovasParaIncluirNormalizada = alunosNovosParaIncluir.Select(a => new Aluno()
                {
                    Nome = a.Nome,
                    RA = a.CodigoAluno,
                    Situacao = a.SituacaoAluno,
                    NomeSocial = a.NomeSocial,
                    Sexo = a.Sexo,
                    DataNascimento = a.DataNascimento,
                    TurmaId = turmaSerapDtos.FirstOrDefault(b => b.Codigo == a.TurmaCodigo.ToString()).Id
                }).ToList();

                await mediator.Send(new InserirAlunosCommand(alunosNovasParaIncluirNormalizada));
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
                        var turmaFix =  await mediator.Send(new ObterTurmaSerapPorIdQuery(alunoAntigo.TurmaId));
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
                                var turmaParaAlunoNovo = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaCodigoParaBuscar));
                                if (turmaParaAlunoNovo == null)
                                    throw new NegocioException($"Turma não localizada para o aluno {alunoQuePodeAlterar.CodigoAluno}");

                                turmaId = turmaParaAlunoNovo.Id;
                            }
                                
                            turmaId = turmaNova.Id;
                        }

                        listaParaAlterar.Add(new Aluno()
                        {
                            Id = alunoAntigo.Id,
                            Nome = alunoQuePodeAlterar.Nome,
                            RA = alunoQuePodeAlterar.CodigoAluno,
                            Situacao = alunoQuePodeAlterar.SituacaoAluno,
                            NomeSocial = alunoQuePodeAlterar.NomeSocial,
                            Sexo = alunoQuePodeAlterar.Sexo,
                            DataNascimento = alunoQuePodeAlterar.DataNascimento,
                            TurmaId = turmaId
                        });
                    }
                }

                if (listaParaAlterar.Any())
                    await mediator.Send(new AlterarAlunosCommand(listaParaAlterar));
            }
        }
    }
}
