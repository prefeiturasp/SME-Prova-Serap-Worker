using MediatR;
using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class InserirDresCommand : IRequest<bool>
    {
        public InserirDresCommand(IEnumerable<Dre> dres)
        {
            Dres = dres;
        }

        public IEnumerable<Dre> Dres { get; set; }
    }
}
