using MessagePack;
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
        public async Task SalvarRedisAsync(string nomeChave, object valor, int minutosParaExpirar = 720)
        {
            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();
            {
                await distributedCache.SetAsync(nomeChave, MessagePackSerializer.Serialize(valor), new DistributedCacheEntryOptions()
                                                .SetAbsoluteExpiration(TimeSpan.FromMinutes(minutosParaExpirar)));

                timer.Stop();
            }
        }

        public async Task<T> ObterRedisAsync<T>(string nomeChave, Func<Task<T>> buscarDados, int minutosParaExpirar = 720)
        {
            var inicioOperacao = DateTime.UtcNow;
            var timer = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var byteCache = await distributedCache.GetAsync(nomeChave);

                timer.Stop();

                if (byteCache != null)
                {
                    return MessagePackSerializer.Deserialize<T>(byteCache);
                }

                var dados = await buscarDados();

                await SalvarRedisAsync(nomeChave, dados, minutosParaExpirar);

                return dados;

            }
            catch (Exception ex)
            {
                servicoLog.Registrar(ex);
                timer.Stop();
                return default;
            }
        }
    }
}
