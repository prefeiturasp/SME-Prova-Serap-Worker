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
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmaSerapPorIdQueryHandler(IRepositorioCache repositorioCache, IRepositorioTurma repositorioTurma)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<Turma> Handle(ObterTurmaSerapPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioCache.ObterRedisAsync($"turma-{request.Id}", () => repositorioTurma.ObterPorIdAsync(request.Id), 60);
        }
    }
}
