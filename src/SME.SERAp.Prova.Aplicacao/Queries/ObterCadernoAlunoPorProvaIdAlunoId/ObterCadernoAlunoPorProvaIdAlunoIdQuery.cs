using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterCadernoAlunoPorProvaIdAlunoIdQuery : IRequest<CadernoAluno>
    {
        public ObterCadernoAlunoPorProvaIdAlunoIdQuery(long provaId, long alunoId)
        {
            ProvaId = provaId;
            AlunoId = alunoId;
        }

        public long ProvaId { get; set; }
        public long AlunoId { get; set; }
    }
}
