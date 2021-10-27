using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class InserirDreCommand : IRequest<bool>
    {
        public InserirDreCommand(Dre dre)
        {
            Dre = dre;
        }

        public Dre Dre { get; set; }
    }
}
