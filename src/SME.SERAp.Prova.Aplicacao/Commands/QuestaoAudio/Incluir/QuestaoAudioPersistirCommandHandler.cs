using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;


namespace SME.SERAp.Prova.Aplicacao
{
    public class QuestaoAudioPersistirCommandHandler : IRequestHandler<QuestaoAudioPersistirCommand, long>
    {
        private readonly IRepositorioQuestaoAudio repositorioQuestaoAudio;

        public QuestaoAudioPersistirCommandHandler(IRepositorioQuestaoAudio repositorioQuestaoAudio)
        {
            this.repositorioQuestaoAudio = repositorioQuestaoAudio ?? throw new System.ArgumentNullException(nameof(repositorioQuestaoAudio));
        }

        public async Task<long> Handle(QuestaoAudioPersistirCommand request, CancellationToken cancellationToken)
        {
            return await repositorioQuestaoAudio.IncluirAsync(new Dominio.QuestaoAudio(request.ArquivoId, request.QuestaoId));
        }
    }
}
