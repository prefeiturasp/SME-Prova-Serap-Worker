using System.Threading.Tasks;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioQuestaoAlunoTai : IRepositorioBase<QuestaoAlunoTai>
    {
        Task<bool> RemoverQuestaoAlunoTaiPorProvaIdAsync(long provaId);
        Task<bool> ExisteQuestaoAlunoTaiPorAlunoId(long alunoId);
        Task<int> ExcluirQuestaoAlunoTai(long provaId, long alunoRa);
    }
}