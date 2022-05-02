using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmaPorCodigoQueryHandler : IRequestHandler<ObterTurmaPorCodigoQuery, TurmaAtribuicaoDto>
    {
        private readonly IRepositorioCache repositorioCache;
        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmaPorCodigoQueryHandler(IRepositorioCache repositorioCache, IRepositorioTurma repositorioTurma)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<TurmaAtribuicaoDto> Handle(ObterTurmaPorCodigoQuery request, CancellationToken cancellationToken)
        {
            return await this.repositorioCache.ObterRedisAsync($"turma-cod-{request.Codigo}", () => repositorioTurma.ObterTurmaAtribuicaoPorCodigo(request.AnoLetivo, request.Codigo), 5);
        }
    }
}
