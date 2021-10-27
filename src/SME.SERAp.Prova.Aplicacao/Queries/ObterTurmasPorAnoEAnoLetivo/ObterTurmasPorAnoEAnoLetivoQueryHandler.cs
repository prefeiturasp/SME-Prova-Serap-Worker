using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmasPorAnoEAnoLetivoQueryHandler : IRequestHandler<ObterTurmasPorAnoEAnoLetivoQuery, IEnumerable<Turma>>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmasPorAnoEAnoLetivoQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<IEnumerable<Turma>> Handle(ObterTurmasPorAnoEAnoLetivoQuery request, CancellationToken cancellationToken)
            => await repositorioTurma.ObterTurmasPorAnoEAnoLetivo(request.Ano, request.AnoLetivo);
    }
}
