using MediatR;
using SME.SERAp.Prova.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExcluirDownloadProvaUseCase : IExcluirDownloadProvaUseCase
    {
        private readonly IMediator mediator;

        public ExcluirDownloadProvaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var downloadProva = mensagemRabbit.ObterObjetoMensagem<ExcluirDownloadProvaAlunoDto>();
            await mediator.Send(new ExcluirDownloadsProvaAlunoCommand(downloadProva.Ids, downloadProva.DataAlteracao));
            return true;
        }
    }
}
