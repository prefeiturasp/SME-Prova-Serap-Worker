using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ProvaAtualizarCommand : IRequest<long>
    {
        public ProvaAtualizarCommand(Dominio.Prova prova)
        {
            Prova = prova;
        }

        public Dominio.Prova Prova { get; set; }
    }
}
