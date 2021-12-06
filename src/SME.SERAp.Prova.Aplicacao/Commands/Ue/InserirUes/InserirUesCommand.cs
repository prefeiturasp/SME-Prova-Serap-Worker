using MediatR;
using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class InserirUesCommand : IRequest<bool>
    {
        public InserirUesCommand(IEnumerable<Ue> ues)
        {
            Ues = ues;
        }

        public IEnumerable<Ue> Ues { get; set; }
    }
}
