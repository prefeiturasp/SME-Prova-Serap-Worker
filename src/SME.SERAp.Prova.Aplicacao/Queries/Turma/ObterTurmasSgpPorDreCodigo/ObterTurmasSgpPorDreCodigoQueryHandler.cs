using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmasSgpPorDreCodigoQueryHandler : IRequestHandler<ObterTurmasSgpPorDreCodigoQuery, IEnumerable<TurmaSgpDto>>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmasSgpPorDreCodigoQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<IEnumerable<TurmaSgpDto>> Handle(ObterTurmasSgpPorDreCodigoQuery request, CancellationToken cancellationToken)
            => await repositorioTurma.ObterTurmasSgpPorDreCodigoAsync(request.DreCodigo);
    }
}
