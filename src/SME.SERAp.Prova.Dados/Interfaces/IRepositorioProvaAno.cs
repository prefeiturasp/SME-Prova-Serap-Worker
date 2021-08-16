using SME.SERAp.Prova.Dominio;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioProvaAno : IRepositorioBase<ProvaAno>
    {
        Task<bool> RemoverAnosPorProvaIdAsync(long provaId);
    }
}
