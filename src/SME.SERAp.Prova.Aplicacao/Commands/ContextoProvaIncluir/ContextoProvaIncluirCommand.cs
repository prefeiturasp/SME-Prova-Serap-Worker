using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ContextoProvaIncluirCommand : IRequest<long>
    {
        public ContextoProvaIncluirCommand(Dominio.ContextoProva contextoProva)
        {
            ContextoProva = contextoProva;
        }

        public Dominio.ContextoProva ContextoProva { get; set; }
    }
}
