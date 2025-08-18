using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.Commands
{
    public class LimparCacheProvaTaiCommandHandler : IRequestHandler<LimparCacheProvaTaiCommand, bool>
    {
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioAluno repositorioAluno;
        public LimparCacheProvaTaiCommandHandler(IRepositorioCache repositorioCache, IRepositorioAluno repositorioAluno)
        {
            this.repositorioCache = repositorioCache;
            this.repositorioAluno = repositorioAluno;
        }

        public async Task<bool> Handle(LimparCacheProvaTaiCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var aluno = await repositorioAluno.ObterAlunoPorCodigo(request.AlunoRa);
                if (aluno?.Id is null)
                    return false;

                await repositorioCache.RemoverRedisAsync(string.Format(CacheChave.AlunoProva, request.ProvaId, request.AlunoRa));
                await repositorioCache.RemoverRedisAsync(string.Format(CacheChave.UltimaProficienciaProva, request.AlunoRa, request.ProvaId));
                await repositorioCache.RemoverRedisAsync(string.Format(CacheChave.QuestaoAmostraTaiAluno, request.AlunoRa, request.ProvaId));
                await repositorioCache.RemoverRedisAsync(string.Format(CacheChave.RespostaAmostraTaiAluno, request.AlunoRa, request.ProvaId));
                await repositorioCache.RemoverRedisAsync(string.Format(CacheChave.QuestaoAdministradoTaiAluno, aluno.Id, request.ProvaId));

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
