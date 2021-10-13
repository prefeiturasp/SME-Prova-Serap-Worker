using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioQuestaoArquivo : IRepositorioBase<QuestaoArquivo>
    {
        Task<bool> RemoverPorIdsAsync(long[] ids);
        Task<IEnumerable<QuestaoArquivo>> ObterArquivosPorProvaIdAsync(long id);
    }
}
