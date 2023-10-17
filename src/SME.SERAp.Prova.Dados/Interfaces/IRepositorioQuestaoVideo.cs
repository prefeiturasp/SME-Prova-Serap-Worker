using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioQuestaoVideo : IRepositorioBase<QuestaoVideo>
    {
        Task<IEnumerable<QuestaoVideo>> ObterPorQuestaoId(long questaoId);
        Task<IEnumerable<QuestaoVideo>> ObterPorProvaId(long provaId);
        Task<bool> RemoverPorIdsAsync(long[] ids);
        Task<long> ObterQuestaoVideoIdPorQuestaoIdArquivoVideoId(long questaoId, long arquivoVideoId);
    }
}
