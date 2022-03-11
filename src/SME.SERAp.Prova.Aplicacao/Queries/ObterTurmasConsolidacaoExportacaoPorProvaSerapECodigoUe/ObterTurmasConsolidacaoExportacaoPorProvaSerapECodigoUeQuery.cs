using MediatR;
using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;


namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmasConsolidacaoExportacaoPorProvaSerapECodigoUeQuery : IRequest<IEnumerable<Turma>>
    {
        public ObterTurmasConsolidacaoExportacaoPorProvaSerapECodigoUeQuery(long provaSerap, string codigoUe)
        {
            ProvaSerap = provaSerap;
            CodigoUe = codigoUe;
        }

        public long ProvaSerap { get; set; }
        public string CodigoUe { get; set; }        
    }
}
