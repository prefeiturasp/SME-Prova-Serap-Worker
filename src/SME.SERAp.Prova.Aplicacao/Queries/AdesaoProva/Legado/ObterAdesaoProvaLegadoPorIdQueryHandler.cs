using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAdesaoProvaLegadoPorIdQueryHandler : IRequestHandler<ObterAdesaoProvaLegadoPorIdQuery, IEnumerable<ProvaAdesaoEntityDto>>
    {

        private readonly IRepositorioProvaAdesaoLegado repositorioProvaAdesaoLegado;

        public ObterAdesaoProvaLegadoPorIdQueryHandler(IRepositorioProvaAdesaoLegado repositorioProvaAdesaoLegado)
        {
            this.repositorioProvaAdesaoLegado = repositorioProvaAdesaoLegado ?? throw new ArgumentNullException(nameof(repositorioProvaAdesaoLegado));
        }

        public async Task<IEnumerable<ProvaAdesaoEntityDto>> Handle(ObterAdesaoProvaLegadoPorIdQuery request, CancellationToken cancellationToken)
            => await repositorioProvaAdesaoLegado.ObterAdesaoPorProvaId(request.ProvaLegadoId);
    }
}
