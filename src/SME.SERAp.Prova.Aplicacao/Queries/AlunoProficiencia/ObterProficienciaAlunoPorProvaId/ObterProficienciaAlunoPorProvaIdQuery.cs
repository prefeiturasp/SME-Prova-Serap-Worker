using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterProficienciaAlunoPorProvaIdQuery : IRequest<decimal>
    {
        public ObterProficienciaAlunoPorProvaIdQuery(long provaId, long alunoId)
        {
            ProvaId = provaId;
            AlunoId = alunoId;
        }

        public long ProvaId { get; set; }
        public long AlunoId { get; set; }
    }
}
