using MediatR;
using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmasPorAnoEAnoLetivoQuery : IRequest<IEnumerable<Turma>>
    {
        public ObterTurmasPorAnoEAnoLetivoQuery(string ano, int anoLetivo)
        {
            Ano = ano;
            AnoLetivo = anoLetivo;
        }

        public string Ano { get; set; }
        public int AnoLetivo { get; set; }
    }
}
