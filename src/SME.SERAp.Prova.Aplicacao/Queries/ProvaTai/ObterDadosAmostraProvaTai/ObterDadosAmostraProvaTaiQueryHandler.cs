using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterDadosAmostraProvaTaiQueryHandler : IRequestHandler<ObterDadosAmostraProvaTaiQuery, IEnumerable<AmostraProvaTaiDto>>
    {   
        private readonly IRepositorioProvaLegado repositorioProvaLegado;
        private readonly IRepositorioCache repositorioCache;

        public ObterDadosAmostraProvaTaiQueryHandler(IRepositorioProvaLegado repositorioProvaLegado, IRepositorioCache repositorioCache)
        {
            this.repositorioProvaLegado = repositorioProvaLegado ?? throw new ArgumentNullException(nameof(repositorioProvaLegado));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<IEnumerable<AmostraProvaTaiDto>> Handle(ObterDadosAmostraProvaTaiQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCache.ObterRedisAsync(CacheChave.DadosAmostraProvaTai,
                () => repositorioProvaLegado.ObterDadosAmostraProvaTai(request.ProvaLegadoId));
        }
    }
}
