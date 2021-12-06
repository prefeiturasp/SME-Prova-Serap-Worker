using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmaSerapPorIdQueryHandler : IRequestHandler<ObterTurmaSerapPorIdQuery, Turma>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmaSerapPorIdQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<Turma> Handle(ObterTurmaSerapPorIdQuery request, CancellationToken cancellationToken)
            => await repositorioTurma.ObterPorIdAsync(request.Id);
    }
}
