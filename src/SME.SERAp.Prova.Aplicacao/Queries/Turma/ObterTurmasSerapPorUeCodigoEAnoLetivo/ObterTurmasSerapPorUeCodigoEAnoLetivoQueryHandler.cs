using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmasSerapPorUeCodigoEAnoLetivoQueryHandler : IRequestHandler<ObterTurmasSerapPorUeCodigoEAnoLetivoQuery, IEnumerable<TurmaSgpDto>>
    {

        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmasSerapPorUeCodigoEAnoLetivoQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<IEnumerable<TurmaSgpDto>> Handle(ObterTurmasSerapPorUeCodigoEAnoLetivoQuery request, CancellationToken cancellationToken)
            => await repositorioTurma.ObterTurmasSerapPorUeCodigoEAnoLetivoAsync(request.UeCodigo, request.AnoLetivo);
    }
}
