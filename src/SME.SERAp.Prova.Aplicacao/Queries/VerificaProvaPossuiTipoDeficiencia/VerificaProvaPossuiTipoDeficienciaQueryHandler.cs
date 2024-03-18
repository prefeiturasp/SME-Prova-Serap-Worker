using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace SME.SERAp.Prova.Aplicacao.Queries.VerificaProvaPossuiTipoDeficiencia
{
    public class VerificaProvaPossuiTipoDeficienciaQueryHandler : IRequestHandler<VerificaProvaPossuiTipoDeficienciaQuery, bool>
    {
        private readonly IRepositorioProva repositorioProva;

        public VerificaProvaPossuiTipoDeficienciaQueryHandler(IRepositorioProva repositorioProva)
        {
            this.repositorioProva = repositorioProva ?? throw new ArgumentNullException(nameof(repositorioProva));
        }

        public async Task<bool> Handle(VerificaProvaPossuiTipoDeficienciaQuery request, CancellationToken cancellationToken)
        {
           return await repositorioProva.VerificaSePossuiTipoDeficiencia(request.ProvaLegadoId);

        }
    }
}