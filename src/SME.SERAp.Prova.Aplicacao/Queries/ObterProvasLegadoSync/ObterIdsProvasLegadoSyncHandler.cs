using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio.Dtos;

namespace SME.SERAp.Prova.Aplicacao.Queries.ObterProvasLegadoSync
{
    public class ObterIdsProvasLegadoSyncHandler :
        IRequestHandler<ObterIdsProvasLegadoSyncQuery, ObterIdsProvaLegadoDto>
    {
        private readonly IRepositorioProvaLegado _repositorioProvaLegado;

        public ObterIdsProvasLegadoSyncHandler(IRepositorioProvaLegado repositorioProvaLegado)
        {
            _repositorioProvaLegado = repositorioProvaLegado;
        }

        public Task<ObterIdsProvaLegadoDto> Handle(ObterIdsProvasLegadoSyncQuery request,
            CancellationToken cancellationToken)
            => _repositorioProvaLegado.ObterIds();
    }
}