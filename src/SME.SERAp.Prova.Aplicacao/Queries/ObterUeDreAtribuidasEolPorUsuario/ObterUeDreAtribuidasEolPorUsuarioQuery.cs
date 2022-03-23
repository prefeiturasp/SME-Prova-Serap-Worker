using System.Collections.Generic;
using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUeDreAtribuidasEolPorUsuarioQuery : IRequest<IEnumerable<string>>
    {
        public ObterUeDreAtribuidasEolPorUsuarioQuery(string codigoRf)
        {
            CodigoRf = codigoRf;
        }

        public string CodigoRf { get; set; }
    }
}
