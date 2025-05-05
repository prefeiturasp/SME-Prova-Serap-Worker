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
        Task<IEnumerable<Questao>> ObterQuestoesComImagemNaoSincronizadasTEMP(long provaId);
        Task<IEnumerable<QuestaoAtualizada>> ObterQuestoesAtualizadas(long provaId, int pagina, int quantidade);
        Task<QuestaoCompletaDto> MontarQuestaoCompletaPorIdAsync(long id);
        Task<long> ObterIdQuestaoPorProvaIdCadernoLegadoId(long provaId, string caderno, long questaoLegadoId);
        Task<IEnumerable<ResumoQuestaoProvaDto>> ObterResumoQuestoesPorProvaIdParaCacheAsync(long provaId);
        Task<IEnumerable<long>> ObterIdsQuestoesPorProvaIdCadernoAsync(long provaId, string caderno);
    }
}
