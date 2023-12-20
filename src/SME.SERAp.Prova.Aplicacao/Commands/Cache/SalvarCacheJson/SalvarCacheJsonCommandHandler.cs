using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;

namespace SME.SERAp.Prova.Aplicacao
{
    public class SalvarCacheJsonCommandHandler : IRequestHandler<SalvarCacheJsonCommand, bool>
    {
        private readonly IRepositorioCache repositorioCache;

        public SalvarCacheJsonCommandHandler(IRepositorioCache repositorioCache)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<bool> Handle(SalvarCacheJsonCommand request, CancellationToken cancellationToken)
        {
            var minutosParaExpirar = 0;
            if (request.MinutosParaExpirar != null)
                minutosParaExpirar = request.MinutosParaExpirar.GetValueOrDefault(); 

            if (minutosParaExpirar > 0)
                await repositorioCache.SalvarRedisToJsonAsync(request.NomeCache, request.Json, minutosParaExpirar);
            else
                await repositorioCache.SalvarRedisToJsonAsync(request.NomeCache, request.Json);
            
            return true;
        }
    }
}