using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterDrePorCodigoQueryHandler : IRequestHandler<ObterDrePorCodigoQuery, Dre>
    {
        private readonly IRepositorioDre repositorioDre;

        public ObterDrePorCodigoQueryHandler(IRepositorioDre repositorioDre)
        {
            this.repositorioDre = repositorioDre ?? throw new ArgumentNullException(nameof(repositorioDre));
        }

        public async Task<Dre> Handle(ObterDrePorCodigoQuery request, CancellationToken cancellationToken)
            => await repositorioDre.ObterDREPorCodigo(request.DreCodigo);
    }
}
