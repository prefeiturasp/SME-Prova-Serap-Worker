﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlternativarProvaPorProvaIdQueryHandler :
        IRequestHandler<ObterAlternativarProvaPorProvaIdQuery, IEnumerable<AlternativasProvaIdDto>>
    {
        private readonly IRepositorioProvaLegado repositorioProvaLegado;

        public ObterAlternativarProvaPorProvaIdQueryHandler(IRepositorioProvaLegado repositorioProvaLegado)
        {
            this.repositorioProvaLegado = repositorioProvaLegado ??
                                          throw new ArgumentNullException(nameof(repositorioProvaLegado));
        }

        public async Task<IEnumerable<AlternativasProvaIdDto>> Handle(ObterAlternativarProvaPorProvaIdQuery request,
            CancellationToken cancellationToken)
            => await repositorioProvaLegado.ObterAlternativasPorIdDaProva(request.ProvaId);
    }
}