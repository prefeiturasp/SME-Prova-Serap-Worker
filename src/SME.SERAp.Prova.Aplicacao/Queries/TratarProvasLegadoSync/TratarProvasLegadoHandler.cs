using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio.Dtos;

namespace SME.SERAp.Prova.Aplicacao.Queries.ObterProvasLegadoSync
{
    public class TratarProvasLegadoHandler :
        IRequestHandler<TratarProvasLegadoQuery, ObterIdsProvaLegadoDto>
    {
        private readonly IRepositorioProvaLegado _repositorioProvaLegado;

        public TratarProvasLegadoHandler(IRepositorioProvaLegado repositorioProvaLegado)
        {
            _repositorioProvaLegado = repositorioProvaLegado;
        }

        public Task<ObterIdsProvaLegadoDto> Handle(TratarProvasLegadoQuery request,
            CancellationToken cancellationToken)
            => _repositorioProvaLegado.ObterIds();
    }
}