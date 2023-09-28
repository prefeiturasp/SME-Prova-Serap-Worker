using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioQuestao : IRepositorioBase<Questao>
    {
        Task<Questao> ObterPorIdLegadoAsync(long id);
        Task<Questao> ObterPorIdEProvaIdLegadoAsync(long id, long provaId);
        Task<bool> RemoverPorProvaIdAsync(long provaId);
        Task<IEnumerable<Questao>> ObterQuestoesComImagemNaoSincronizadas();
        Task<IEnumerable<QuestaoAtualizada>> ObterQuestoesAtualizadas(int pagina, int quantidade);
        Task<QuestaoCompletaDto> MontarQuestaoCompletaPorIdAsync(long id);
    }
}
