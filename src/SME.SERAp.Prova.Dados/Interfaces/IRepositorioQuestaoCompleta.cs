using SME.SERAp.Prova.Dominio;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioQuestaoCompleta : IRepositorioBase<QuestaoCompleta>
    {
        Task IncluirOuUpdateAsync(QuestaoCompleta questaoCompleta);
    }
}
