using System.Collections.Generic;
using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUeDreAtribuidasEolPorUsuarioQueryHandler : IRequestHandler<ObterUeDreAtribuidasEolPorUsuarioQuery, IEnumerable<string>>
    {
        private readonly IRepositorioParametroSistema repositorioParametroSistema;
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioGeralEol repositorioGeralEol;

        public ObterUeDreAtribuidasEolPorUsuarioQueryHandler(IRepositorioGeralEol repositorioGeralEol, 
            IRepositorioParametroSistema repositorioParametroSistema, 
            IRepositorioCache repositorioCache)
        {
            this.repositorioGeralEol = repositorioGeralEol ?? throw new ArgumentNullException(nameof(repositorioGeralEol));
            this.repositorioParametroSistema = repositorioParametroSistema ?? throw new ArgumentNullException(nameof(repositorioParametroSistema));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<IEnumerable<string>> Handle(ObterUeDreAtribuidasEolPorUsuarioQuery request, CancellationToken cancellationToken)
        {
            var parametrosDoSistema = await repositorioCache.ObterRedisAsync(CacheChave.Parametros, async () => await repositorioParametroSistema.ObterTudoAsync(), 1440);
            if (parametrosDoSistema == null || !parametrosDoSistema.Any())
                throw new Exception("Obter abrangência EOL - Não foi possível obter os parâmetros de sistema.");
            
            parametrosDoSistema = parametrosDoSistema.Where(a => a.Ano == DateTime.Now.Year && (int)TipoParametroSistema.TipoEscolaSerap == (int)a.Tipo);
            if (parametrosDoSistema == null || !parametrosDoSistema.Any())
                throw new Exception($"Obter abrangência EOL - Não foi possível obter os parâmetros de sistema com o ano {DateTime.Now.Year} e tipo escola {(int)TipoParametroSistema.TipoEscolaSerap}.");            

            var tiposEscola = parametrosDoSistema.FirstOrDefault().Valor.Split(",").Select(c => int.Parse(c)).ToArray();
            var ueDreAtribuidasEol = await repositorioCache.ObterRedisAsync(string.Format(CacheChave.UeDreAtribuidasEolUsuario, request.CodigoRf), async () => await repositorioGeralEol.ObterUeDreAtribuidasEolAsync(request.CodigoRf, tiposEscola), 60);
            
            return ueDreAtribuidasEol;
        }
    }
}
