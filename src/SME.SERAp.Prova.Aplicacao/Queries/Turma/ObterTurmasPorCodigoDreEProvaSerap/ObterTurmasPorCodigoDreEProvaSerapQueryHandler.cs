using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmasPorCodigoDreEProvaSerapQueryHandler : IRequestHandler<ObterTurmasPorCodigoDreEProvaSerapQuery, IEnumerable<Turma>>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmasPorCodigoDreEProvaSerapQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<IEnumerable<Turma>> Handle(ObterTurmasPorCodigoDreEProvaSerapQuery request, CancellationToken cancellationToken)
        {
            return await repositorioTurma.ObterTurmasPorCodigoDreEProvaSerap(request.CodigoDre, request.ProvaSerap);
        }
    }
}