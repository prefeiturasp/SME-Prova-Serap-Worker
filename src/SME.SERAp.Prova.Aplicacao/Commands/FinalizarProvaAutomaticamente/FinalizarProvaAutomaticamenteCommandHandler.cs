using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class FinalizarProvaAutomaticamenteCommandHandler : IRequestHandler<FinalizarProvaAutomaticamenteCommand, bool>
    {
        private readonly IRepositorioProva repositorioProva;
        public FinalizarProvaAutomaticamenteCommandHandler(IRepositorioProva repositorioProva)
        {
            this.repositorioProva = repositorioProva ?? throw new ArgumentNullException(nameof(repositorioProva));
        }

        public async Task<bool> Handle(FinalizarProvaAutomaticamenteCommand request, CancellationToken cancellationToken)
        {
            return await repositorioProva.FinalizarProvaAsync(request.ProvaParaFinalizar);
        }
    }
}
