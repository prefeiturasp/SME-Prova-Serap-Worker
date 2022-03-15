using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmaPorCodigosQueryHandler : IRequestHandler<ObterTurmaPorCodigosQuery, IEnumerable<Turma>>
    {
        public readonly IRepositorioTurma repositorioTurma;

        public ObterTurmaPorCodigosQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<IEnumerable<Turma>> Handle(ObterTurmaPorCodigosQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTurma.ObterTurmasPorCodigos(request.Codigos);
        }
    }
}
