using System;

namespace SME.SERAp.Prova.Infra
{
    public class DownloadProvaAlunoExcluirDto
    {
        public Guid[] Codigos { get; set; }
        public DateTime DataAlteracao { get; set; }
    }
}
