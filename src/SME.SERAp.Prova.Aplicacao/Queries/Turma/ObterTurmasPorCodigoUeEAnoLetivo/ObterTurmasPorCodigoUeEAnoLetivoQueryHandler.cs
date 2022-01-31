using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmasPorCodigoUeEAnoLetivoQueryHandler : IRequestHandler<ObterTurmasPorCodigoUeEAnoLetivoQuery, IEnumerable<Turma>>
    {

        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmasPorCodigoUeEAnoLetivoQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<IEnumerable<Turma>> Handle(ObterTurmasPorCodigoUeEAnoLetivoQuery request, CancellationToken cancellationToken)
            => await repositorioTurma.ObterTurmasPorCodigoUeEAnoLetivo(request.CodigoUe, request.AnoLetivo);

    }
}
