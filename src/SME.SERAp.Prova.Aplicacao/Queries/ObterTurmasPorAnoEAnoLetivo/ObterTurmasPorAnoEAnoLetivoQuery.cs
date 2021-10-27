using MediatR;
using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmasPorAnoEAnoLetivoQuery : IRequest<IEnumerable<Turma>>
    {
        public ObterTurmasPorAnoEAnoLetivoQuery(int ano, int anoLetivo)
        {
            Ano = ano;
            AnoLetivo = anoLetivo;
        }

        public int Ano { get; set; }
        public int AnoLetivo { get; set; }
    }
}
