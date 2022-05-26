using MediatR;
using SME.SERAp.Prova.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarDownloadProvaAlunoUseCase : ITratarDownloadProvaAlunoUseCase
    {
        private readonly IMediator mediator;

        public TratarDownloadProvaAlunoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var downloadProvaAlunoTratar = mensagemRabbit.ObterObjetoMensagem<DownloadProvaAlunoTratarDto>();
            return downloadProvaAlunoTratar.Situacao switch
            {
                Dominio.DownloadProvaAlunoSituacao.Incluir => await mediator.Send(new IncluirDownloadProvaCommand(downloadProvaAlunoTratar.DownloadProvaAlunoDto)),
                Dominio.DownloadProvaAlunoSituacao.Excluir => await mediator.Send(new ExcluirDownloadsProvaAlunoCommand(downloadProvaAlunoTratar.DownloadProvaAlunoExcluirDto)),
                _ => throw new Exception("Situação do download não tratado"),
            };
        }
    }
}
