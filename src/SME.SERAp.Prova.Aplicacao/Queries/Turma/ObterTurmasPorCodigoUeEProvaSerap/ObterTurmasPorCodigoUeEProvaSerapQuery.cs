using MediatR;
using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmasPorCodigoUeEProvaSerapQuery : IRequest<IEnumerable<Turma>>
    {
        public ObterTurmasPorCodigoUeEProvaSerapQuery(string codigoUe, long provaSerap)
        {
            CodigoUe = codigoUe;
            ProvaSerap = provaSerap;
        }

        public string CodigoUe { get; set; }
        public long ProvaSerap { get; set; }
    }
}
