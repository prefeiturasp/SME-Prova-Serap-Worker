using System.Collections.Generic;
using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterParametrosParaCacheQuery : IRequest<IEnumerable<ParametroSistema>>
    {
    }
}