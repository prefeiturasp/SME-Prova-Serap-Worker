using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioQuestaoAudio : IRepositorioBase<QuestaoAudio>
    {
        Task<IEnumerable<QuestaoAudio>> ObterPorQuestaoId(long questaoId);
        Task<IEnumerable<QuestaoAudio>> ObterPorProvaId(long provaId);
        Task<bool> RemoverPorIdsAsync(long[] ids);
        Task<long> ObterQuestaoAudioIdPorArquivoId(long arquivoId);
    }
}
