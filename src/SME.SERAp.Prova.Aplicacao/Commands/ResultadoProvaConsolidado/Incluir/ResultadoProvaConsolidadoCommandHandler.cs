﻿using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SME.SERAp.Prova.Aplicacao.Commands.ResultadoProvaConsolidado.Incluir
{
    public class ResultadoProvaConsolidadoCommandHandler : IRequestHandler<ResultadoProvaConsolidadoCommand, bool>
    {
        private readonly IRepositorioResultadoProvaConsolidado repositorioResultadoProvaConsolidado;

        public ResultadoProvaConsolidadoCommandHandler(IRepositorioResultadoProvaConsolidado repositorioResultadoProvaConsolidado)
        {
            this.repositorioResultadoProvaConsolidado = repositorioResultadoProvaConsolidado ?? throw new ArgumentException(nameof(repositorioResultadoProvaConsolidado));
        }

        public async Task<bool> Handle(ResultadoProvaConsolidadoCommand request, CancellationToken cancellationToken)
        {
            await repositorioResultadoProvaConsolidado.IncluirResultadoProvaConsolidado(request.ResultadoProvaConsolidado);
            return true;
        }
    }
}