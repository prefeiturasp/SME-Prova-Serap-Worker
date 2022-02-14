using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class VerificaProvaPossuiRespostasPorProvaIdQueryHandler : IRequestHandler<VerificaProvaPossuiRespostasPorProvaIdQuery, bool>
    {
        private readonly IRepositorioProva repositorioProva;

        public VerificaProvaPossuiRespostasPorProvaIdQueryHandler(IRepositorioProva repositorioProva)
        {
            this.repositorioProva = repositorioProva ?? throw new ArgumentNullException(nameof(repositorioProva));
        }
        public async Task<bool> Handle(VerificaProvaPossuiRespostasPorProvaIdQuery request, CancellationToken cancellationToken)
        {
            var existeProvaFinalizada = await repositorioProva.VerificaSeExisteProvaFinalizadaPorId(request.ProvaId);
            if (existeProvaFinalizada)
                return true;

            var existeRespostas = await repositorioProva.VerificaSeExisteRespostasPorId(request.ProvaId);
            return existeRespostas;
        }
    }
}
