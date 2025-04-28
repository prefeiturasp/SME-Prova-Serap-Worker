using MediatR;

namespace SME.SERAp.Prova.Aplicacao.Queries
{
    public class VerificaProvaAlunoDuplicadaQuery : IRequest<bool>
    {
        public VerificaProvaAlunoDuplicadaQuery(long alunoRa, long provaId)
        {
            AlunoRa = alunoRa;
            ProvaId = provaId;
        }

        public long AlunoRa { get; set; }

        public long ProvaId { get; set; }
    }
}
