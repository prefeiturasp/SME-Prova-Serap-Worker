using System.Collections.Generic;
using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterProvasLiberadasNoPeriodoParaCacheQuery : IRequest<IEnumerable<Dominio.Prova>>
    {
    }
}