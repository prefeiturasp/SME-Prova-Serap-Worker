using MediatR;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Dominio;
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
            try
            {
                var turmas = mensagemRabbit.ObterObjetoMensagem<List<TurmaParaSincronizacaoInstitucionalDto>>();
                if (turmas == null || !turmas.Any())
                    throw new NegocioException("Não foi possível localizar as Turmas para sincronizar os alunos.");

                var turmasCodigos = turmas.Select(c => long.Parse(c.Codigo)).ToArray();
                var todosAlunosTurmaEol = await mediator.Send(new ObterAlunosEolPorTurmasCodigoQuery(turmasCodigos));
                if (todosAlunosTurmaEol == null || !todosAlunosTurmaEol.Any())
                    throw new NegocioException("Não foi possível localizar os alunos no Eol para a sincronização instituicional.");

                var alunosEolParaTratarCodigos = todosAlunosTurmaEol.Select(a => a.CodigoAluno).Distinct().ToArray();
                var todosAlunosSerap = await mediator.Send(new ObterAlunosSerapPorCodigosQuery(alunosEolParaTratarCodigos));

                foreach (var turma in turmas)
                {
                    var alunosTurmaEol = todosAlunosTurmaEol.Where(c => c.TurmaCodigo.ToString() == turma.Codigo);

                    var tasks = new[]
                    {
                        TratarInclusao(alunosTurmaEol, todosAlunosSerap, turma.Id),
                        TratarAlteracao(alunosTurmaEol, todosAlunosSerap, turma),
                    };

                    // -> processamento concorrente
                    Task.WaitAll(tasks);

                    await TratarInativo(alunosTurmaEol, turma.Id);
                }

                var turmasIds = turmas.Select(c => c.Id).Distinct().ToArray();
                await Tratar(turmasIds);
            }
            catch (Exception e)
            {
                servicoLog.Registrar($"Erro ao sincronizar os Alunos: {mensagemRabbit.Mensagem}", e);
                throw;
            }

            return true;
        }

        private async Task TratarInclusao(IEnumerable<AlunoEolDto> alunosTurmaEol, IEnumerable<Aluno> alunosTurmaSerap,
            long turmaId)
        {
            var alunosTurmaEolCodigo = alunosTurmaEol.Select(c => c.CodigoAluno).Distinct();
            var alunosTurmaSerapCodigo = alunosTurmaSerap.Select(c => c.RA).Distinct();

            var alunosNovosCodigos = alunosTurmaEolCodigo.Where(a => !alunosTurmaSerapCodigo.Contains(a));

            if (alunosNovosCodigos.Any())
            {
                var alunosNovosParaIncluir = alunosTurmaEol.Where(a => alunosNovosCodigos.Contains(a.CodigoAluno));

                if (alunosNovosParaIncluir.Any())
                {
                    var alunosNovosParaIncluirNormalizada = alunosNovosParaIncluir.Select(a => new Aluno
                        {
                            Nome = a.Nome,
                            RA = a.CodigoAluno,
                            Situacao = a.SituacaoAluno,
                            NomeSocial = a.NomeSocial,
                            Sexo = a.Sexo,
                            DataNascimento = a.DataNascimento,
                            TurmaId = turmaId,
                            DataAtualizacao = a.DataSituacao
                    }).ToList();
                    
                    await mediator.Send(new InserirAlunosCommand(alunosNovosParaIncluirNormalizada));
                }
            }
        }

        private async Task TratarAlteracao(IEnumerable<AlunoEolDto> alunosTurmaEol, IEnumerable<Aluno> alunosTurmaSerap,
            TurmaParaSincronizacaoInstitucionalDto turma)
        {
            var alunosTurmaEolCodigo = alunosTurmaEol.Select(c => c.CodigoAluno).Distinct();
            var alunosTurmaSerapCodigo = alunosTurmaSerap.Select(c => c.RA).Distinct();

            var alunosParaAlterarCodigos = alunosTurmaEolCodigo.Where(a => alunosTurmaSerapCodigo.Contains(a));

            if (alunosParaAlterarCodigos.Any())
            {
                var alunosQuePodemAlterar = alunosTurmaEol.Where(a => alunosParaAlterarCodigos.Contains(a.CodigoAluno));

                var listaParaAlterar = new List<Aluno>();

                foreach (var alunoQuePodeAlterar in alunosQuePodemAlterar)
                {
                    var alunoAntigo = alunosTurmaSerap.FirstOrDefault(a => a.RA == alunoQuePodeAlterar.CodigoAluno);

                    if (alunoAntigo == null)
                        continue;
                    
                    var turmaFix = await mediator.Send(new ObterTurmaSerapPorIdQuery(alunoAntigo.TurmaId));

                    if (turmaFix == null)
                        continue;

                    var turmaAntigaDoAluno = new TurmaSgpDto { Id = turmaFix.Id, Codigo = turmaFix.Codigo };

                    if (turmaFix.ModalidadeCodigo == (int)Modalidade.EJA && turma.ModalidadeCodigo == (int)Modalidade.EJA
                                                                         && turmaFix.Semestre > turma.Semestre
                                                                         && turmaFix.AnoLetivo == turma.AnoLetivo
                                                                         && turmaFix.Codigo != turma.Codigo)
                    {
                        alunoQuePodeAlterar.TurmaCodigo = long.Parse(turmaAntigaDoAluno.Codigo);
                    }

                    //-> Atualiza somente se for a ultima situação do aluno do ano letivo mais atual.
                    if (turmaFix.AnoLetivo >= turma.AnoLetivo &&
                        (turmaFix.AnoLetivo != turma.AnoLetivo ||
                         alunoAntigo.DataAtualizacao > alunoQuePodeAlterar.DataSituacao))
                    {
                        continue;
                    }

                    //-> Valida se existe alguma informação a ser alterada.
                    if (alunoAntigo.Nome == alunoQuePodeAlterar.Nome &&
                        alunoAntigo.Situacao == alunoQuePodeAlterar.SituacaoAluno &&
                        alunoAntigo.DataNascimento.Date == alunoQuePodeAlterar.DataNascimento.Date &&
                        alunoAntigo.NomeSocial?.ToString() == alunoQuePodeAlterar.NomeSocial?.ToString() &&
                        alunoAntigo.Sexo == alunoQuePodeAlterar.Sexo &&
                        turmaAntigaDoAluno.Codigo == alunoQuePodeAlterar.TurmaCodigo.ToString())
                    {
                        continue;
                    }

                    var turmaId = alunoAntigo.TurmaId;
                    
                    if (turmaAntigaDoAluno.Codigo != alunoQuePodeAlterar.TurmaCodigo.ToString())
                    {
                        turmaId = turma.Id;

                        var alunosParaTratar = new List<AlunoParaSincronizacaoInstitucionalDto>
                        {
                            new AlunoParaSincronizacaoInstitucionalDto(alunoAntigo.Id, alunoQuePodeAlterar.CodigoAluno,
                                turmaId)
                        };

                        await mediator.Send(new PublicaFilaRabbitCommand(
                            RotasRabbit.SincronizaEstruturaInstitucionalTurmaAlunoHistoricoTratar,
                            alunosParaTratar));
                    }

                    listaParaAlterar.Add(new Aluno
                    {
                        Id = alunoAntigo.Id,
                        Nome = alunoQuePodeAlterar.Nome,
                        RA = alunoQuePodeAlterar.CodigoAluno,
                        Situacao = alunoQuePodeAlterar.SituacaoAluno,
                        NomeSocial = alunoQuePodeAlterar.NomeSocial,
                        Sexo = alunoQuePodeAlterar.Sexo,
                        DataAtualizacao = alunoQuePodeAlterar.DataSituacao,
                        DataNascimento = alunoQuePodeAlterar.DataNascimento,
                        TurmaId = turmaId
                    });
                }

                if (listaParaAlterar.Any())
                {
                    await mediator.Send(new AlterarAlunosCommand(listaParaAlterar));
                    await mediator.Send(new RemoverAlunosCacheCommand(listaParaAlterar.Select(x => x.RA).Distinct().ToArray()));
                }
            }
        }

        private async Task TratarInativo(IEnumerable<AlunoEolDto> alunosTurmaEol, long turmaId)
        {
            var alunosTurmaSerap = await mediator.Send(new ObterAlunosSerapPorTurmasIdsQuery(turmaId));
            if (alunosTurmaSerap.Any())
            {
                var alunosInativos = alunosTurmaSerap.Where(t => alunosTurmaEol.All(x => x.CodigoAluno != t.RA));
                if (alunosInativos.Any())
                {
                    await mediator.Send(new InativarAlunosCommand(turmaId, alunosInativos));
                    await mediator.Send(new RemoverAlunosCacheCommand(alunosInativos.Select(x => x.RA).Distinct().ToArray()));
                }
            }
        }

        private async Task Tratar(long[] turmasIds)
        {
            var todosAlunosSerap = await mediator.Send(new ObterAlunosSerapPorTurmasIdsQuery(turmasIds));

            foreach (var aluno in todosAlunosSerap)
            {
                await mediator.Send(new PublicaFilaRabbitCommand(
                    RotasRabbit.SincronizaEstruturaInstitucionalAlunoTratar,
                    new AlunoParaSincronizacaoInstitucionalDto(aluno.Id, aluno.RA, aluno.TurmaId)));
            }
        }
    }
}
