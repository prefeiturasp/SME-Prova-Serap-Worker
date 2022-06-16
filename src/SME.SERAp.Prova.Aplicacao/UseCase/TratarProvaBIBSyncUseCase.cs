using MediatR;
using Sentry;
using SME.SERAp.Prova.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProvaBIBSyncUseCase : ITratarProvaBIBSyncUseCase
    {
        private readonly IMediator mediator;

        public TratarProvaBIBSyncUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {

            SentrySdk.AddBreadcrumb("Iniciou a fila ");

            var provaCadernosAlunos = await mediator.Send(new ObterAlunosSemCadernoProvaBibQuery());

            SentrySdk.AddBreadcrumb("capturou os alunos que não possui caderno. Total: " + provaCadernosAlunos.Count());
            if (provaCadernosAlunos != null && provaCadernosAlunos.Any())
            {
                foreach (var prova in provaCadernosAlunos)
                {
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ProvaBIBTratar, prova));
                    SentrySdk.AddBreadcrumb($"Enviou para tratar provaId:{prova.ProvaId}, aluno:{prova.AlunoId}");
                }
            }

            return true;
        }
    }
}