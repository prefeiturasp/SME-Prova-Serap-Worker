using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class RemoverAlunosCacheCommandHandler : IRequestHandler<RemoverAlunosCacheCommand, bool>
    {
        private readonly IRepositorioCache repositorioCache;

        public RemoverAlunosCacheCommandHandler(IRepositorioCache repositorioCache)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<bool> Handle(RemoverAlunosCacheCommand request, CancellationToken cancellationToken)
        {
            foreach (var ra in request.AlunosRA)
            {
                await repositorioCache.RemoverRedisAsync(string.Format(CacheChave.MeusDados, ra));
                await repositorioCache.RemoverRedisAsync(string.Format(CacheChave.Aluno, ra));
                await repositorioCache.RemoverRedisAsync(string.Format(CacheChave.ProvasAnteriorAluno, ra));
                await repositorioCache.RemoverRedisAsync(string.Format(CacheChave.PreferenciasAluno, ra));
                await repositorioCache.RemoverRedisAsync(string.Format(CacheChave.AlunoTurma, ra));
            }

            return true;
        }
    }
}
