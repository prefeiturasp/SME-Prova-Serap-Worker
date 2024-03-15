using MediatR;
using SME.SERAp.Prova.Aplicacao.Commands.ResultadoProvaConsolidado.Incluir;
using SME.SERAp.Prova.Dados;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SME.SERAp.Prova.Aplicacao.Commands.ResultadoProvaConsolidado.Excluir
{
    public class ExcluirResultadoProvaConsolidadosPorProvaLegadoIdCommandHandler : IRequestHandler<ExcluirResultadoProvaConsolidadosPorProvaLegadoIdCommand, bool>
    {
        private readonly IRepositorioResultadoProvaConsolidado repositorioProvaConsolidado;

        public ExcluirResultadoProvaConsolidadosPorProvaLegadoIdCommandHandler(IRepositorioResultadoProvaConsolidado repositorioProvaConsolidado)
        {
            this.repositorioProvaConsolidado = repositorioProvaConsolidado ?? throw new ArgumentException(nameof(repositorioProvaConsolidado));
        }

        public async Task<bool> Handle(ExcluirResultadoProvaConsolidadosPorProvaLegadoIdCommand request, CancellationToken cancellationToken)
        {
            await repositorioProvaConsolidado.ExcluirDadosConsolidadosPorProvaLegadoId(request.ProvaLegadoId);
            return true;
        }
    }
}