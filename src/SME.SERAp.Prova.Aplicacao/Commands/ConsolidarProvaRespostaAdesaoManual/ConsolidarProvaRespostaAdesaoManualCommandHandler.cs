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
        private readonly IRepositorioResultadoProvaConsolidado repositorioProvaConsolidado;

        public ConsolidarProvaRespostaAdesaoManualCommandHandler(IRepositorioProva repositorioProva, IRepositorioResultadoProvaConsolidado repositorioProvaConsolidado)
        {
            this.repositorioProva = repositorioProva ?? throw new ArgumentNullException(nameof(repositorioProva));
            this.repositorioProvaConsolidado = repositorioProvaConsolidado ?? throw new ArgumentNullException(nameof(repositorioProvaConsolidado)); ; 
        }
        public async Task<bool> Handle(ConsolidarProvaRespostaAdesaoManualCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await repositorioProvaConsolidado.ExcluirDadosConsolidadosPorProvaSerapEstudantesId(request.ProvaId);
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
