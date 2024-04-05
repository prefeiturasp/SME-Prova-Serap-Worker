using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterParametrosParaCacheQueryHandler : IRequestHandler<ObterParametrosParaCacheQuery, IEnumerable<ParametroSistema>>
    {
        private readonly IRepositorioParametroSistema repositorioParametroSistema;

        public ObterParametrosParaCacheQueryHandler(IRepositorioParametroSistema repositorioParametroSistema)
        {
            this.repositorioParametroSistema = repositorioParametroSistema ?? throw new ArgumentNullException(nameof(repositorioParametroSistema));
        }

        public async Task<IEnumerable<ParametroSistema>> Handle(ObterParametrosParaCacheQuery request, CancellationToken cancellationToken)
        {
            return await repositorioParametroSistema.ObterTodosParaCacheAsync();
        }
    }
}