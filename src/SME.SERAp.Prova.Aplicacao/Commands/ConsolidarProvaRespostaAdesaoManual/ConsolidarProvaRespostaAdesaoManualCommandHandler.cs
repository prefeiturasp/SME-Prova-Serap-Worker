using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SME.SERAp.Prova.Aplicacao.Commands.ConsolidarProvaRespostaAdesaoManual
{
    internal class ConsolidarProvaRespostaAdesaoManualCommandHandler : IRequestHandler<ConsolidarProvaRespostaAdesaoManualCommand, bool>
    {
        private readonly IRepositorioProva repositorioProva;

        public ConsolidarProvaRespostaAdesaoManualCommandHandler(IRepositorioProva repositorioProva)
        {
            this.repositorioProva = repositorioProva ?? throw new ArgumentNullException(nameof(repositorioProva));
        }
        public async Task<bool> Handle(ConsolidarProvaRespostaAdesaoManualCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await repositorioProva.LimparDadosConsolidadosPorProvaSerapEstudantesId(request.ProvaId);
                await repositorioProva.ConsolidarProvaRespostasAdesaoManual(request.ProvaId);

                return true;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
