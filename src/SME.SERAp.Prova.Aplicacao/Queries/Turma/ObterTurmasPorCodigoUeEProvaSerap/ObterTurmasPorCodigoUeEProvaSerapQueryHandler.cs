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
    public class ObterTurmasPorCodigoUeEProvaSerapQueryHandler : IRequestHandler<ObterTurmasPorCodigoUeEProvaSerapQuery, IEnumerable<Turma>>
    {

        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmasPorCodigoUeEProvaSerapQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<IEnumerable<Turma>> Handle(ObterTurmasPorCodigoUeEProvaSerapQuery request, CancellationToken cancellationToken)
            => await repositorioTurma.ObterTurmasPorCodigoUeEProvaSerap(request.CodigoUe, request.ProvaSerap);
    }
}
