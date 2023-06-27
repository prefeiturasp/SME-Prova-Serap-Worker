using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExcluirDadosConsolidadoCommandHandler : IRequestHandler<ExcluirDadosConsolidadoCommand, int>
    {
        private readonly IRepositorioProva repositorioProva;

        public ExcluirDadosConsolidadoCommandHandler(IRepositorioProva repositorioProva)
        {
            this.repositorioProva = repositorioProva ?? throw new ArgumentNullException(nameof(repositorioProva));
        }
        public async Task<int> Handle(ExcluirDadosConsolidadoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await repositorioProva.ExcluirDadosConsolidadoPaginado(request.ProvaLegadoId, request.Take, request.Skip);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
