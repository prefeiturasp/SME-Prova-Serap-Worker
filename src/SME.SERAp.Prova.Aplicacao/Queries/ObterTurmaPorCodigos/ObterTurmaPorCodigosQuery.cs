using MediatR;
using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmaPorCodigosQuery : IRequest<IEnumerable<Turma>>
    {
        public ObterTurmaPorCodigosQuery(string[] codigos)
        {
            Codigos = codigos;
        }

        public string[] Codigos { get; set; }
    }
}
