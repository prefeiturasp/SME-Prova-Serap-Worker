using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmasSgpPorDreCodigoEAnoLetivoQueryHandler : IRequestHandler<ObterTurmasSgpPorDreCodigoEAnoLetivoQuery, IEnumerable<TurmaSgpDto>>
    {

        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmasSgpPorDreCodigoEAnoLetivoQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<IEnumerable<TurmaSgpDto>> Handle(ObterTurmasSgpPorDreCodigoEAnoLetivoQuery request, CancellationToken cancellationToken)
                => await repositorioTurma.ObterTurmasSgpPorDreCodigoEAnoLetivoAsync(request.DreCodigo, request.AnoLetivo, request.Historica);
    }
}
