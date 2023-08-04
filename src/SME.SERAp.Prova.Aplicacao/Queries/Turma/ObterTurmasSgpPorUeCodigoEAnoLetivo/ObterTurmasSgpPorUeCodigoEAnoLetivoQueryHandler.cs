using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmasSgpPorUeCodigoEAnoLetivoQueryHandler : IRequestHandler<ObterTurmasSgpPorUeCodigoEAnoLetivoQuery, IEnumerable<TurmaSgpDto>>
    {

        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmasSgpPorUeCodigoEAnoLetivoQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<IEnumerable<TurmaSgpDto>> Handle(ObterTurmasSgpPorUeCodigoEAnoLetivoQuery request, CancellationToken cancellationToken)
                => await repositorioTurma.ObterTurmasSgpPorUeCodigoEAnoLetivoAsync(request.UeCodigo, request.AnoLetivo, request.Historica);
    }
}
