using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class QuestaoVideoPersistirCommandHandler : IRequestHandler<QuestaoVideoPersistirCommand, long>
    {

        private readonly IRepositorioQuestaoVideo repositorioQuestaoVideo;

        public QuestaoVideoPersistirCommandHandler(IRepositorioQuestaoVideo repositorioQuestaoVideo)
        {
            this.repositorioQuestaoVideo = repositorioQuestaoVideo ?? throw new System.ArgumentNullException(nameof(repositorioQuestaoVideo));
        }

        public async Task<long> Handle(QuestaoVideoPersistirCommand request, CancellationToken cancellationToken)
        {
            return await repositorioQuestaoVideo.IncluirAsync(request.QuestaoVideo);
        }
    }
}
