using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class SalvarCacheCommandCommandHandler : IRequestHandler<SalvarCacheCommandCommand, bool>
    {
        private readonly IRepositorioCache repositorioCache;

        public SalvarCacheCommandCommandHandler(IRepositorioCache repositorioCache)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<bool> Handle(SalvarCacheCommandCommand request, CancellationToken cancellationToken)
        {
            var minutosParaExpirar = 0;
            if (request.MinutosParaExpirar != null)
                minutosParaExpirar = request.MinutosParaExpirar.GetValueOrDefault(); 

            if (minutosParaExpirar > 0)
                await repositorioCache.SalvarRedisAsync(request.NomeCache, request.Valor, minutosParaExpirar);
            else
                await repositorioCache.SalvarRedisAsync(request.NomeCache, request.Valor);
            
            return true;
        }
    }
}
