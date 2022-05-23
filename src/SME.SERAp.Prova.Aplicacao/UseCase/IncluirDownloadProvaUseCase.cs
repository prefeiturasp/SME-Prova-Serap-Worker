using MediatR;
using SME.SERAp.Prova.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class IncluirDownloadProvaUseCase : IIncluirDownloadProvaUseCase
    {
        private readonly IMediator mediator;

        public IncluirDownloadProvaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var downloadProvaAluno = mensagemRabbit.ObterObjetoMensagem<DownloadProvaAlunoDto>();
            await mediator.Send(new IncluirDownloadProvaCommand(downloadProvaAluno));
            return true;
        }
    }
}
