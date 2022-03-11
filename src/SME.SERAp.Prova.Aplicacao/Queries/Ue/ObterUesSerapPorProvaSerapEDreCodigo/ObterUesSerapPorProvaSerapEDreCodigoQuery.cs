using MediatR;
using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;


namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUesSerapPorProvaSerapEDreCodigoQuery : IRequest<IEnumerable<Ue>>
    {
        public ObterUesSerapPorProvaSerapEDreCodigoQuery(long provaSerap, string dreCodigo)
        {
            ProvaSerap = provaSerap;
            DreCodigo = dreCodigo;            
        }

        public long ProvaSerap { get; set; }
        public string DreCodigo { get; set; }
    }
}
