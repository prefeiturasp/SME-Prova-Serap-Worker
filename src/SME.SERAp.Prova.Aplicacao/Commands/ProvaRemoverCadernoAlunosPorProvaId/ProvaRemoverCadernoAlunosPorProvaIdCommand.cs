using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ProvaRemoverCadernoAlunosPorProvaId : IRequest<bool>
    {
        public ProvaRemoverCadernoAlunosPorProvaId(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}
