using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExcluirQuestaoAudioPorQuestaoIdCommandHandler : IRequestHandler<ExcluirQuestaoAudioPorQuestaoIdCommand, bool>
    {

        private readonly IRepositorioQuestaoAudio repositorioQuestaoAudio;
        private readonly IRepositorioArquivo repositorioArquivo;

        public ExcluirQuestaoAudioPorQuestaoIdCommandHandler(IRepositorioQuestaoAudio repositorioQuestaoAudio, IRepositorioArquivo repositorioArquivo)
        {
            this.repositorioQuestaoAudio = repositorioQuestaoAudio ?? throw new System.ArgumentNullException(nameof(repositorioQuestaoAudio));
            this.repositorioArquivo = repositorioArquivo ?? throw new System.ArgumentNullException(nameof(repositorioArquivo));
        }

        public async Task<bool> Handle(ExcluirQuestaoAudioPorQuestaoIdCommand request, CancellationToken cancellationToken)
        {
            var audiosQuestao = await repositorioQuestaoAudio.ObterPorQuestaoId(request.QuestaoId);
            await repositorioQuestaoAudio.RemoverPorIdsAsync(audiosQuestao.Select(a => a.Id).ToArray());
            await repositorioArquivo.RemoverPorIdsAsync(audiosQuestao.Select(a => a.ArquivoId).ToArray());

            return true;
        }
    }
}
