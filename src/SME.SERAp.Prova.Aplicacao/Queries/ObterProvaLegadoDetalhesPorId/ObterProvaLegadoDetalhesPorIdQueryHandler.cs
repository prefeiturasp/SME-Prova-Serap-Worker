﻿using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterProvaLegadoDetalhesPorIdQueryHandler : IRequestHandler<ObterProvaLegadoDetalhesPorIdQuery, ProvaLegadoDetalhesIdDto>
    {
        private readonly IRepositorioProvaLegado repositorioProvaLegado;

        public ObterProvaLegadoDetalhesPorIdQueryHandler(IRepositorioProvaLegado repositorioProvaLegado)
        {
            this.repositorioProvaLegado = repositorioProvaLegado ?? throw new ArgumentNullException(nameof(repositorioProvaLegado));
        }
        public async Task<ProvaLegadoDetalhesIdDto> Handle(ObterProvaLegadoDetalhesPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioProvaLegado.ObterDetalhesPorId(request.ProvaId);
        }
    }
}
