using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using SME.SERAp.Prova.Infra.Interfaces;
using SME.SERAp.Prova.Infra.Utils;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados.Cache
{
    public class RepositorioCache : IRepositorioCache
    {

        private readonly IServicoLog servicoLog;
        private readonly IDistributedCache distributedCache;

        public RepositorioCache(IServicoLog servicoLog, IDistributedCache distributedCache)
        {

            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
            this.distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
        }

        public async Task RemoverRedisAsync(string nomeChave)
        {
            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                await distributedCache.RemoveAsync(nomeChave);

                timer.Stop();
            }
            catch (Exception ex)
            {
                timer.Stop();
                servicoLog.Registrar(ex);
            }
        }
    }
}
