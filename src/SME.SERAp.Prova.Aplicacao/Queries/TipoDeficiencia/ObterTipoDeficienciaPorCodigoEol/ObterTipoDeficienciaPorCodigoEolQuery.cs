using MediatR;
using SME.SERAp.Prova.Dominio;
using System;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTipoDeficienciaPorCodigoEolQuery : IRequest<TipoDeficiencia>
    {

        public int CodigoEol { get; private set; }

        public ObterTipoDeficienciaPorCodigoEolQuery(int codigoEol)
        {
            CodigoEol = codigoEol;
        }
    }
}
