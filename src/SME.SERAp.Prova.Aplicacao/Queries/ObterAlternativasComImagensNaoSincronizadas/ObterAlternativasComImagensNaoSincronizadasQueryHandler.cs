using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlternativasComImagensNaoSincronizadasQueryHandler : IRequestHandler<ObterAlternativasComImagensNaoSincronizadasQuery, IEnumerable<Alternativa>>
    {
        private readonly IRepositorioAlternativa repositorioAlternativa;

        public ObterAlternativasComImagensNaoSincronizadasQueryHandler(IRepositorioAlternativa repositorioAlternativa)
        {
            this.repositorioAlternativa = repositorioAlternativa ?? throw new ArgumentNullException(nameof(repositorioAlternativa));
        }
        public async Task<IEnumerable<Alternativa>> Handle(ObterAlternativasComImagensNaoSincronizadasQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAlternativa.ObterAlternativasComImagemNaoSincronizadas();
        }
    }
}
