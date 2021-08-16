using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ProvaIncluirCommand : IRequest<long>
    {
        public ProvaIncluirCommand(Dominio.Prova prova)
        {
            Prova = prova;
        }

        public Dominio.Prova Prova { get; set; }
    }
}
