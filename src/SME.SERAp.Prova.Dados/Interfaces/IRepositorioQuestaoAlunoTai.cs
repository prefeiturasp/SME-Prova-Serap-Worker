using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioQuestaoAlunoTai : IRepositorioBase<QuestaoAlunoTai>
    {
        Task<bool> RemoverQuestaoAlunoTaiPorProvaIdAsync(long provaId);

        Task<IEnumerable<QuestaoAlunoTai>> ObterQuestoesAlunoTaiPorAlunoRaProvaId(long alunoRa, long provaId);
        Task<bool> ExisteQuestaoAlunoTaiPorAlunoId(long alunoId);
    }
}