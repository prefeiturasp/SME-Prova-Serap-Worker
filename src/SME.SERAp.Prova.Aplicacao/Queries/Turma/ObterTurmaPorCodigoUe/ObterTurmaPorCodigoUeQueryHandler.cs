using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmaPorCodigoUeQueryHandler : IRequestHandler<ObterTurmaPorCodigoUeQuery, Turma>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmaPorCodigoUeQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<Turma> Handle(ObterTurmaPorCodigoUeQuery request, CancellationToken cancellationToken)
            => await repositorioTurma.ObterturmaPorCodigo(request.TurmaCodigo);
    }
}
