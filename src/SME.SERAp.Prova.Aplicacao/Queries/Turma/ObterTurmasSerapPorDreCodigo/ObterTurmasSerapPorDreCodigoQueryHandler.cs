using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmasSerapPorDreCodigoQueryHandler : IRequestHandler<ObterTurmasSerapPorDreCodigoQuery, IEnumerable<TurmaSgpDto>>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmasSerapPorDreCodigoQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<IEnumerable<TurmaSgpDto>> Handle(ObterTurmasSerapPorDreCodigoQuery request, CancellationToken cancellationToken)
            => await repositorioTurma.ObterTurmasSerapPorDreCodigoAsync(request.DreCodigo);
    }
}
