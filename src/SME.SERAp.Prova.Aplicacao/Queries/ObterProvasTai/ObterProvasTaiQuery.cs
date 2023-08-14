using System.Collections.Generic;
using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterProvasTaiQuery : IRequest<IEnumerable<ProvaTaiSyncDto>>
    {
    }
}