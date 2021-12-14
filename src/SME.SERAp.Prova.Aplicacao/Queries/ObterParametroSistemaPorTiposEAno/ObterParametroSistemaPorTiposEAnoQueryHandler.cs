using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterParametroSistemaPorTiposEAnoQueryHandler : IRequestHandler<ObterParametroSistemaPorTiposEAnoQuery, IEnumerable<ParametroSistema>>
    {
        private readonly IRepositorioParametroSistema repositorioParametroSistema;

        public ObterParametroSistemaPorTiposEAnoQueryHandler(IRepositorioParametroSistema repositorioParametroSistema)
        {
            this.repositorioParametroSistema = repositorioParametroSistema ?? throw new System.ArgumentNullException(nameof(repositorioParametroSistema));
        }
        public async Task<IEnumerable<ParametroSistema>> Handle(ObterParametroSistemaPorTiposEAnoQuery request, CancellationToken cancellationToken)
        {
            var parametrosDoSistema = await repositorioParametroSistema.ObterTudoAsync();

            if (parametrosDoSistema != null && parametrosDoSistema.Any())
                return parametrosDoSistema.Where(a => a.Ano == request.Ano && request.Tipos.Contains((int)a.Tipo));
            else
                return default;
        }
    }
}
