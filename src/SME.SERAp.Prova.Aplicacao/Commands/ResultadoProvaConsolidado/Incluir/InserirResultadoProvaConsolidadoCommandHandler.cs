using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace SME.SERAp.Prova.Aplicacao
{
    public class InserirResultadoProvaConsolidadoCommandHandler : IRequestHandler<InserirResultadoProvaConsolidadoCommand, bool>
    {
        private readonly IRepositorioResultadoProvaConsolidado repositorioResultadoProvaConsolidado;

        public InserirResultadoProvaConsolidadoCommandHandler(IRepositorioResultadoProvaConsolidado repositorioResultadoProvaConsolidado)
        {
            this.repositorioResultadoProvaConsolidado = repositorioResultadoProvaConsolidado ?? throw new ArgumentException(nameof(repositorioResultadoProvaConsolidado));
        }

        public async Task<bool> Handle(InserirResultadoProvaConsolidadoCommand request, CancellationToken cancellationToken)
        {
            await repositorioResultadoProvaConsolidado.IncluirResultadoProvaConsolidado(request.ResultadoProvaConsolidado);
            return true;
        }
    }
}