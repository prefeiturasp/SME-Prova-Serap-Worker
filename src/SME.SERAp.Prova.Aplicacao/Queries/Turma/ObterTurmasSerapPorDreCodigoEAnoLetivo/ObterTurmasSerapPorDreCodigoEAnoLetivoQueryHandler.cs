using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmasSerapPorDreCodigoEAnoLetivoQueryHandler : IRequestHandler<ObterTurmasSerapPorDreCodigoEAnoLetivoQuery, IEnumerable<TurmaSgpDto>>
    {

        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmasSerapPorDreCodigoEAnoLetivoQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<IEnumerable<TurmaSgpDto>> Handle(ObterTurmasSerapPorDreCodigoEAnoLetivoQuery request, CancellationToken cancellationToken)
            => await repositorioTurma.ObterTurmasSerapPorDreCodigoEAnoLetivoAsync(request.DreCodigo, request.AnoLetivo);
    }
}
