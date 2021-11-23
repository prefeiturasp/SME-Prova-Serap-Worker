using SME.SERAp.Prova.Dominio;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioContextoProva : IRepositorioBase<ContextoProva>
    {
        Task<bool> RemoverContextosProvaPorProvaIdAsync(long provaId);
    }
}
