using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterProvasPorUltimaAtualizacaoQueryHandler : IRequestHandler<ObterProvasPorUltimaAtualizacaoQuery, IEnumerable<ProvaAtualizadaDto>>
    {
        private readonly IRepositorioProva repositorioProva;

        public ObterProvasPorUltimaAtualizacaoQueryHandler(IRepositorioProva repositorioProva)
        {
            this.repositorioProva = repositorioProva ?? throw new ArgumentNullException(nameof(repositorioProva));
        }

        public async Task<IEnumerable<ProvaAtualizadaDto>> Handle(ObterProvasPorUltimaAtualizacaoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioProva.ObterProvaPorUltimaAtualizacao(request.DataBase);
        }
    }
}
