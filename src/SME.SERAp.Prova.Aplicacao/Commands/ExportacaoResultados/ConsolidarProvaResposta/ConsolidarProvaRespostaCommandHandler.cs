using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ConsolidarProvaRespostaCommandHandler : IRequestHandler<ConsolidarProvaRespostaCommand, int>
    {
        private readonly IRepositorioProva repositorioProva;

        public ConsolidarProvaRespostaCommandHandler(IRepositorioProva repositorioProva)
        {
            this.repositorioProva = repositorioProva ?? throw new ArgumentNullException(nameof(repositorioProva));
        }
        public async Task<int> Handle(ConsolidarProvaRespostaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await repositorioProva.ConsolidarDadosPaginado(request.ProvaLegadoId, request.AderirTodos, request.ParaEstudanteComDeficiencia, request.Take, request.Skip);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
