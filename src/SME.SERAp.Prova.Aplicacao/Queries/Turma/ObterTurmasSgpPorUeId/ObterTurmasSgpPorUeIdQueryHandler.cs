using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmasSgpPorUeIdQueryHandler : IRequestHandler<ObterTurmasSgpPorUeIdQuery, IEnumerable<TurmaSgpDto>>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmasSgpPorUeIdQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<IEnumerable<TurmaSgpDto>> Handle(ObterTurmasSgpPorUeIdQuery request, CancellationToken cancellationToken)
            => await repositorioTurma.ObterturmasSgpPorUeId(request.UeId);
    }
}
