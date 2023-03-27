using SME.SERAp.Prova.Dominio;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioQuestaoTri : IRepositorioBase<QuestaoTri>
    {
        Task<QuestaoTri> ObterPorQuestaoIdAsync(long questaoId);
        Task<bool> RemoverPorProvaIdAsync(long id);
    }
}
