using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUeDreAtribuidasCoreSsoPorUsuarioEGrupoQueryHandler : IRequestHandler<ObterUeDreAtribuidasCoreSsoPorUsuarioEGrupoQuery, IEnumerable<string>>
    {
        private readonly IRepositorioGeralCoreSso repositorioGeralCoreSso;
        private readonly IRepositorioCache repositorioCache;

        public ObterUeDreAtribuidasCoreSsoPorUsuarioEGrupoQueryHandler(IRepositorioGeralCoreSso repositorioGeralCoreSso, IRepositorioCache repositorioCache)
        {
            this.repositorioGeralCoreSso = repositorioGeralCoreSso ?? throw new ArgumentNullException(nameof(repositorioGeralCoreSso));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<IEnumerable<string>> Handle(ObterUeDreAtribuidasCoreSsoPorUsuarioEGrupoQuery request, CancellationToken cancellationToken)
        {
            var retorno = await repositorioCache.ObterRedisAsync(
                string.Format(CacheChave.UeDreAtribuidasEolUsuario, request.CodigoRf),
                async () => await repositorioGeralCoreSso.ObterUeDreAtribuidasCoreSsoPorUsuarioIdAsync(request.UsuarioIdCoreSso), 60);

            if (retorno != null && retorno.Any())
                return retorno.Where(c => c.GrupoIdCoreSso == request.GrupoIdCoreSso).Select(c => c.UeCodigo).Distinct();
            
            return await repositorioGeralCoreSso.ObterUeDreAtribuidasCoreSso(request.UsuarioIdCoreSso, request.GrupoIdCoreSso);
        }
    }
}
