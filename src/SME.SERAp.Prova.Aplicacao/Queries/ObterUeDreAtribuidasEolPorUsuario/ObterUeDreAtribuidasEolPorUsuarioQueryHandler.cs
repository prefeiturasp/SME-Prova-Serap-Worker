using System.Collections.Generic;
using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System;
using SME.SERAp.Prova.Dominio;

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
            this.repositorioGeralEol = repositorioGeralEol ?? throw new System.ArgumentNullException(nameof(repositorioGeralEol));
            this.repositorioParametroSistema = repositorioParametroSistema ?? throw new System.ArgumentNullException(nameof(repositorioParametroSistema));
            this.repositorioCache = repositorioCache ?? throw new System.ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<IEnumerable<string>> Handle(ObterUeDreAtribuidasEolPorUsuarioQuery request, CancellationToken cancellationToken)
        {
            int[] tiposEscola;
            var parametrosDoSistema = await repositorioCache.ObterRedisAsync("parametros", async () => await repositorioParametroSistema.ObterTudoAsync(), 1440);
            if (parametrosDoSistema != null && parametrosDoSistema.Any())
                parametrosDoSistema = parametrosDoSistema.Where(a => a.Ano == DateTime.Now.Year && (int)TipoParametroSistema.TipoEscolaSerap == (int)a.Tipo);
            else
                throw new Exception("Obter abrangência EOL - Não foi possível obter os parâmetros de sistema.");

            tiposEscola = new int[] { 1, 2, 3, 4, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 22, 23, 25, 26, 27, 28, 29, 30, 31 };

            return await repositorioGeralEol.ObterUeDreAtribuidasEolAsync(request.CodigoRf, parametrosDoSistema.Select(a => a.Valor).FirstOrDefault());
        }
    }
}
