using MediatR;
using System;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExcluirDownloadsProvaAlunoCommand : IRequest<bool>
    {
        public ExcluirDownloadsProvaAlunoCommand(long[] ids, DateTime? dataAlteracao)
        {
            Ids = ids;
            DataAlteracao = dataAlteracao;
        }

        public long[] Ids { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}
