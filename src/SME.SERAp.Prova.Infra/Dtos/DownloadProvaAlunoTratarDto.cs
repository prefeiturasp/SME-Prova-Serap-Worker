using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Infra
{
    public class DownloadProvaAlunoTratarDto
    {
        public DownloadProvaAlunoSituacao Situacao { get; set; }
        public DownloadProvaAlunoDto DownloadProvaAlunoDto { get; set; }
        public DownloadProvaAlunoExcluirDto DownloadProvaAlunoExcluirDto { get; set; }
    }
}
