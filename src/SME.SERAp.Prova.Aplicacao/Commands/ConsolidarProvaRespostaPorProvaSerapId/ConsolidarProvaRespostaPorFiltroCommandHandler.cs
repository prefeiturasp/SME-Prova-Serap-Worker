using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ConsolidarProvaRespostaPorFiltroCommandHandler : IRequestHandler<ConsolidarProvaRespostaPorFiltroCommand, bool>
    {
        private readonly IRepositorioProva repositorioProva;

        public ConsolidarProvaRespostaPorFiltroCommandHandler(IRepositorioProva repositorioProva)
        {
            this.repositorioProva = repositorioProva ?? throw new ArgumentNullException(nameof(repositorioProva));
        }
        public async Task<bool> Handle(ConsolidarProvaRespostaPorFiltroCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await repositorioProva.LimparDadosConsolidadosPorFiltros(request.ProvaId, request.DreEolId, request.UeEolIds);
                await repositorioProva.ConsolidarProvaRespostasPorFiltros(request.ProvaId, request.DreEolId, request.UeEolIds);
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
