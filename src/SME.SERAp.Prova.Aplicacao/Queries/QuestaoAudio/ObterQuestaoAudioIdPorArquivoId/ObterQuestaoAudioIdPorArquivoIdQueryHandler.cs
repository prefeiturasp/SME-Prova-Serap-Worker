using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;

namespace SME.SERAp.Prova.Aplicacao.Queries
{
    public class ObterQuestaoAudioIdPorArquivoIdQueryHandler : IRequestHandler<ObterQuestaoAudioIdPorArquivoIdQuery, long>
    {
        private readonly IRepositorioQuestaoAudio repositorioQuestaoAudio;

        public ObterQuestaoAudioIdPorArquivoIdQueryHandler(IRepositorioQuestaoAudio repositorioQuestaoAudio)
        {
            this.repositorioQuestaoAudio = repositorioQuestaoAudio ?? throw new ArgumentNullException(nameof(repositorioQuestaoAudio));
        }

        public async Task<long> Handle(ObterQuestaoAudioIdPorArquivoIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioQuestaoAudio.ObterQuestaoAudioIdPorArquivoId(request.QuestaoId, request.ArquivoId);
        }
    }
}