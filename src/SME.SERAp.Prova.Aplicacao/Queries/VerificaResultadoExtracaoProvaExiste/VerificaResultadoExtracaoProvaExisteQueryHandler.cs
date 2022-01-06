using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class VerificaResultadoExtracaoProvaExisteQueryHandler : IRequestHandler<VerificaResultadoExtracaoProvaExisteQuery, bool>
    {
        private readonly IRepositorioResultadoProvaConsolidado repositorioResultadoProvaConsolidado;

        public VerificaResultadoExtracaoProvaExisteQueryHandler(IRepositorioResultadoProvaConsolidado repositorioResultadoProvaConsolidado)
        {
            this.repositorioResultadoProvaConsolidado = repositorioResultadoProvaConsolidado ?? throw new ArgumentNullException(nameof(repositorioResultadoProvaConsolidado));
        }

        public async Task<bool> Handle(VerificaResultadoExtracaoProvaExisteQuery request, CancellationToken cancellationToken)
        {
            return await repositorioResultadoProvaConsolidado.VerificaResultadoExtracaoProvaExiste(request.ProvaLegadoId);
        }
    }
}
