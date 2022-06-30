using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunoPorIdQuery : IRequest<Aluno>
    {
        public ObterAlunoPorIdQuery(long alunoId)
        {
            AlunoId = alunoId;
        }

        public long AlunoId { get; set; }
    }
}
