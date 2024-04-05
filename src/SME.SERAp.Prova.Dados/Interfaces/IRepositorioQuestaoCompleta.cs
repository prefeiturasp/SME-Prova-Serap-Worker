using System.Collections.Generic;
using SME.SERAp.Prova.Dominio;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioQuestaoCompleta : IRepositorioBase<QuestaoCompleta>
    {
        Task IncluirOuUpdateAsync(QuestaoCompleta questaoCompleta);
        Task<IEnumerable<QuestaoCompleta>> ObterQuestoesCompletasPorProvaIdParaCacheAsync(long provaId);
        Task<QuestaoCompleta> ObterQuestaoCompletaPorQuestaoIdAsync(long questaoId);
        Task<IEnumerable<QuestaoCompleta>> ObterQuestoesCompletasLegadoPorProvaIdParaCacheAsync(long provaId);        
    }
}
