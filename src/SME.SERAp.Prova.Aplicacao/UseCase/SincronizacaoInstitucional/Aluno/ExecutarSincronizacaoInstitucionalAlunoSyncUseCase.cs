using MediatR;
using Sentry;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
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
            var turma = mensagemRabbit.ObterObjetoMensagem<TurmaSgpDto>();

            if (turma == null)
                throw new NegocioException("Não foi possível localizar o código da turma para tratar o Sync de Alunos.");

            var alunos = await mediator.Send(new ObterAlunosPorTurmaCodigoQuery(long.Parse(turma.Codigo)));

            if (alunos.Any())
            {
                if (turma.TurmaId > 0)
                {
                    foreach (var aluno in alunos)
                    {
                        aluno.TurmaSerapId = turma.TurmaId;

                        var alunoSerap = await mediator.Send(new ObterAlunoPorCodigoQuery(aluno.CodigoAluno));

                        await mediator.Send(new TrataSincronizacaoInstitucionalAlunoCommand(aluno, alunoSerap));
                    }
                }
            }

            return true;
        }
    }
}
