using MediatR;
using SME.SERAp.Prova.Dados;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterProvaLegadoParaSeremSincronizadasQueryHandler : IRequestHandler<ObterProvaLegadoParaSeremSincronizadasQuery, IEnumerable<long>>
    {
        private readonly IRepositorioProvaLegado _repositorioProvaLegado;

        public ObterProvaLegadoParaSeremSincronizadasQueryHandler(IRepositorioProvaLegado repositorioProvaLegado)
        {
            _repositorioProvaLegado = repositorioProvaLegado;
        }

        public Task<IEnumerable<long>> Handle(ObterProvaLegadoParaSeremSincronizadasQuery request, CancellationToken cancellationToken)
            => _repositorioProvaLegado.ObterProvasIdsParaSeremSincronizadasIds(request.UltimaExecucao);
    }
}