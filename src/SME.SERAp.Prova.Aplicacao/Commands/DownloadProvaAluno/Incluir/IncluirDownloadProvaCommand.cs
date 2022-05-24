using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class IncluirDownloadProvaCommand : IRequest<long>
    {
        public IncluirDownloadProvaCommand(DownloadProvaAlunoDto downloadProvaAlunoDto)
        {
            DownloadProvaAlunoDto = downloadProvaAlunoDto;
            Situacao = 1;
        }

        public DownloadProvaAlunoDto DownloadProvaAlunoDto { get; set; }
        public int Situacao { get; set; }

    }
}

