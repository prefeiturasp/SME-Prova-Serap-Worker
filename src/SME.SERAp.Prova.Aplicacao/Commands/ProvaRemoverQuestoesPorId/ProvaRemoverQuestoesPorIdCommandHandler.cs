using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ProvaRemoverQuestoesPorIdCommandHandler : IRequestHandler<ProvaRemoverQuestoesPorIdCommand, bool>
    {
        private readonly IRepositorioQuestao repositorioQuestao;
        private readonly IRepositorioArquivo repositorioArquivo;
        private readonly IRepositorioQuestaoArquivo repositorioQuestaoArquivo;
        private readonly IRepositorioQuestaoAudio repositorioQuestaoAudio;
        private readonly IRepositorioQuestaoVideo repositorioQuestaoVideo;

        public ProvaRemoverQuestoesPorIdCommandHandler(IRepositorioQuestao repositorioQuestao, IRepositorioArquivo repositorioArquivo,
            IRepositorioQuestaoArquivo repositorioQuestaoArquivo, IRepositorioQuestaoAudio repositorioQuestaoAudio,
            IRepositorioQuestaoVideo repositorioQuestaoVideo)
        {
            this.repositorioQuestao = repositorioQuestao ?? throw new ArgumentNullException(nameof(repositorioQuestao));
            this.repositorioArquivo = repositorioArquivo ?? throw new ArgumentNullException(nameof(repositorioArquivo));
            this.repositorioQuestaoArquivo = repositorioQuestaoArquivo ?? throw new ArgumentNullException(nameof(repositorioQuestaoArquivo));
            this.repositorioQuestaoAudio = repositorioQuestaoAudio ?? throw new ArgumentNullException(nameof(repositorioQuestaoAudio));
            this.repositorioQuestaoVideo = repositorioQuestaoVideo ?? throw new ArgumentNullException(nameof(repositorioQuestaoVideo));
        }
        
        public async Task<bool> Handle(ProvaRemoverQuestoesPorIdCommand request, CancellationToken cancellationToken)
        {
            //TODO: IMPLEMENTAR TRANSAÇÃO
            var questoesArquivos = await repositorioQuestaoArquivo.ObterArquivosPorProvaIdAsync(request.Id);
            if (questoesArquivos.Any())
            {
                var idsQuestosArquivos = questoesArquivos.Select(a => a.Id).ToArray();
                await repositorioQuestaoArquivo.RemoverPorIdsAsync(idsQuestosArquivos);
                var idsArquivos = questoesArquivos.Select(a => a.ArquivoId).ToArray();
                await repositorioArquivo.RemoverPorIdsAsync(idsArquivos);
            }

            var questoesAudios = await repositorioQuestaoAudio.ObterPorProvaId(request.Id);
            if (questoesAudios != null && questoesAudios.Any())
            {
                var idsQuestoesAudios = questoesAudios.Select(a => a.Id).ToArray();
                await repositorioQuestaoAudio.RemoverPorIdsAsync(idsQuestoesAudios);
                var idsArquivos = questoesAudios.Select(a => a.ArquivoId).ToArray();
                await repositorioArquivo.RemoverPorIdsAsync(idsArquivos);
            }

            var questaoVideos = await repositorioQuestaoVideo.ObterPorProvaId(request.Id);
            if(questaoVideos != null && questaoVideos.Any())
            {
                var idsVideos = questaoVideos.Select(v => v.Id).ToArray();
                await repositorioQuestaoVideo.RemoverPorIdsAsync(idsVideos);

                var idsArquivosExcluir = questaoVideos.Select(a => a.ArquivoVideoId).ToList();

                var idsArquivosThumbnail = questaoVideos.Where(a => a.ArquivoThumbnailId != null).Select(a => (long)a.ArquivoThumbnailId).ToList();
                idsArquivosExcluir.AddRange(idsArquivosThumbnail);

                var idsArquivosVideoConvertidoId = questaoVideos.Where(a => a.ArquivoVideoConvertidoId != null).Select(a => (long)a.ArquivoVideoConvertidoId).ToList();
                idsArquivosExcluir.AddRange(idsArquivosVideoConvertidoId);

                await repositorioArquivo.RemoverPorIdsAsync(idsArquivosExcluir.ToArray());
            }

            await repositorioQuestao.RemoverPorProvaIdAsync(request.Id);

            return true;
        }
    }
}
