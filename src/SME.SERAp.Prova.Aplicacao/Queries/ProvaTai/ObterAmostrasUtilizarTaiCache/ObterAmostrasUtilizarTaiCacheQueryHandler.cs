using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAmostrasUtilizarTaiCacheQueryHandler : IRequestHandler<ObterAmostrasUtilizarTaiCacheQuery, IEnumerable<ItemAmostraTaiDto>>
    {
        private readonly IRepositorioCache repositorioCache;

        public ObterAmostrasUtilizarTaiCacheQueryHandler(IRepositorioCache repositorioCache)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<IEnumerable<ItemAmostraTaiDto>> Handle(ObterAmostrasUtilizarTaiCacheQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCache.ObterRedisAsync<IEnumerable<ItemAmostraTaiDto>>(string.Format(CacheChave.AmostrasUtilizarProvaTai, request.ProvaLegadoId));
        }
    }
}