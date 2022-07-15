using MediatR;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExecutarSincronizacaoTurmaAlunoHistoricoSyncUseCase : AbstractUseCase, IExecutarSincronizacaoTurmaAlunoHistoricoSyncUseCase
    {
        public ExecutarSincronizacaoTurmaAlunoHistoricoSyncUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var dreCodigo = mensagemRabbit.Mensagem.ToString();

            if (dreCodigo == null)
            {
                var mensagem = $"Não foi possível fazer parse da mensagem para sync de turmas histórico dos alunos da dre {mensagemRabbit.Mensagem}.";
                throw new NegocioException(mensagem);
            }

            var turmasDaDre = await mediator.Send(new ObterTurmasSerapPorDreCodigoQuery(dreCodigo));

            if (turmasDaDre != null && turmasDaDre.Any())
            {
                var turmasDaDreId = turmasDaDre.Select(a => a.Id).Distinct().ToArray();

                var alunosSerap = await mediator.Send(new ObterAlunosSerapPorTurmasCodigoQuery(turmasDaDreId));

                for (int i = 0; i < alunosSerap.Count(); i += 10)
                {
                    var alunosAgrupadosParaTratar = alunosSerap.Skip(i).Take(10);
                    var alunosSerapRa = alunosAgrupadosParaTratar.Select(a => a.RA).Distinct().ToList();
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.SincronizaEstruturaInstitucionalTurmaAlunoHistoricoTratar, alunosSerapRa.ToArray()));
                }
            }
            else throw new NegocioException($"Não foi possível localizar as turmas da Dre {dreCodigo} para fazer sync de turmas histórico dos alunos.");

            return true;

        }
    }
}
