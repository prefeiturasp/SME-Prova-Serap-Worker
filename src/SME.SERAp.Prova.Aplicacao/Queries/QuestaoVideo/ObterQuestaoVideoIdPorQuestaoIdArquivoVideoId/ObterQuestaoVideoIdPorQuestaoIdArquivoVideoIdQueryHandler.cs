using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;

namespace SME.SERAp.Prova.Aplicacao.Queries
{
    public class ObterQuestaoVideoIdPorQuestaoIdArquivoVideoIdQueryHandler : IRequestHandler<ObterQuestaoVideoIdPorQuestaoIdArquivoVideoIdQuery, long>
    {
        private readonly IRepositorioQuestaoVideo repositorioQuestaoVideo;

        public ObterQuestaoVideoIdPorQuestaoIdArquivoVideoIdQueryHandler(IRepositorioQuestaoVideo repositorioQuestaoVideo)
        {
            this.repositorioQuestaoVideo = repositorioQuestaoVideo ?? throw new ArgumentNullException(nameof(repositorioQuestaoVideo));
        }

        public async Task<long> Handle(ObterQuestaoVideoIdPorQuestaoIdArquivoVideoIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioQuestaoVideo.ObterQuestaoVideoIdPorQuestaoIdArquivoVideoId(request.QuestaoId, request.ArquivoVideoId);
        }
    }
}