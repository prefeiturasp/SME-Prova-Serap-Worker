using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ProvaRemoverQuestoesPorIdCommandHandler : IRequestHandler<ProvaRemoverQuestoesPorIdCommand, bool>
    {
        private readonly IRepositorioQuestaoArquivo repositorioQuestaoArquivo;
        private readonly IRepositorioQuestaoAudio repositorioQuestaoAudio;
        private readonly IRepositorioQuestaoVideo repositorioQuestaoVideo;
        private readonly IRepositorioArquivo repositorioArquivo;
        private readonly IRepositorioQuestao repositorioQuestao;        

        public ProvaRemoverQuestoesPorIdCommandHandler(IRepositorioQuestaoArquivo repositorioQuestaoArquivo,
            IRepositorioQuestaoAudio repositorioQuestaoAudio, IRepositorioQuestaoVideo repositorioQuestaoVideo,
            IRepositorioArquivo repositorioArquivo, IRepositorioQuestao repositorioQuestao)
        {
            this.repositorioQuestaoArquivo = repositorioQuestaoArquivo ?? throw new ArgumentNullException(nameof(repositorioQuestaoArquivo));
            this.repositorioQuestaoAudio = repositorioQuestaoAudio ?? throw new ArgumentNullException(nameof(repositorioQuestaoAudio));
            this.repositorioQuestaoVideo = repositorioQuestaoVideo ?? throw new ArgumentNullException(nameof(repositorioQuestaoVideo));            
            this.repositorioArquivo = repositorioArquivo ?? throw new ArgumentNullException(nameof(repositorioArquivo));
            this.repositorioQuestao = repositorioQuestao ?? throw new ArgumentNullException(nameof(repositorioQuestao));            
        }
        
        public async Task<bool> Handle(ProvaRemoverQuestoesPorIdCommand request, CancellationToken cancellationToken)
        {
            await RemoverQuestoesArquivos(request.Id);
            await RemoverQuestoesAudios(request.Id);
            await RemoverQuestoesVideos(request.Id);
            await repositorioQuestao.RemoverPorProvaIdAsync(request.Id);
            return true;
        }
        
        private async Task RemoverQuestoesArquivos(long provaId)
        {
            var questoesArquivos = await repositorioQuestaoArquivo.ObterArquivosPorProvaIdAsync(provaId);
            if (questoesArquivos != null && questoesArquivos.Any())
            {
                var idsQuestoesArquivos = questoesArquivos.Select(a => a.Id);
                await repositorioQuestaoArquivo.RemoverPorIdsAsync(idsQuestoesArquivos.ToArray());

                var idsArquivos = questoesArquivos.Select(a => a.ArquivoId).ToArray();
                await repositorioArquivo.RemoverPorIdsAsync(idsArquivos);
            }
        }
        
        private async Task RemoverQuestoesAudios(long provaId)
        {
            var questoesAudios = await repositorioQuestaoAudio.ObterPorProvaId(provaId);
            if (questoesAudios != null && questoesAudios.Any())
            {
                var idsQuestoesAudios = questoesAudios.Select(a => a.Id);
                await repositorioQuestaoAudio.RemoverPorIdsAsync(idsQuestoesAudios.ToArray());

                var idsArquivos = questoesAudios.Select(a => a.ArquivoId).ToArray();
                await repositorioArquivo.RemoverPorIdsAsync(idsArquivos);
            }
        }
        
        private async Task RemoverQuestoesVideos(long provaId)
        {
            var questoesVideos = await repositorioQuestaoVideo.ObterPorProvaId(provaId);
            if (questoesVideos != null && questoesVideos.Any())
            {
                await repositorioQuestaoVideo.RemoverPorIdsAsync(questoesVideos.Select(c => c.Id).ToArray());                
                
                var idsArquivosQuestoesVideos = new List<long>();
                idsArquivosQuestoesVideos.AddRange(questoesVideos.Select(v => v.ArquivoVideoId));
                idsArquivosQuestoesVideos.AddRange(questoesVideos.Where(a => a.ArquivoThumbnailId != null).Select(a => (long)a.ArquivoThumbnailId));
                idsArquivosQuestoesVideos.AddRange(questoesVideos.Where(a => a.ArquivoVideoConvertidoId != null).Select(a => (long)a.ArquivoVideoConvertidoId));

                var idsArquivos = idsArquivosQuestoesVideos.ToArray();
                await repositorioArquivo.RemoverPorIdsAsync(idsArquivos);
            }            
        }        
    }
}
