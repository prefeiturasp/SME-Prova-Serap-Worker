using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExcluirAdesaoPorProvaIdCommand : IRequest<bool>
    {
        public long ProvaId { get; set; }

        public ExcluirAdesaoPorProvaIdCommand(long provaId)
        {
            ProvaId = provaId;
        }
    }
}
