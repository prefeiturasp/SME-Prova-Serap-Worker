using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExcluirDownloadsProvaAlunoCommand : IRequest<bool>
    {
        public ExcluirDownloadsProvaAlunoCommand(DownloadProvaAlunoExcluirDto downloadProvaAlunoExcluirDto)
        {
            DownloadProvaAlunoExcluirDto = downloadProvaAlunoExcluirDto;
        }
        public DownloadProvaAlunoExcluirDto DownloadProvaAlunoExcluirDto { get; set; }
    }
}
