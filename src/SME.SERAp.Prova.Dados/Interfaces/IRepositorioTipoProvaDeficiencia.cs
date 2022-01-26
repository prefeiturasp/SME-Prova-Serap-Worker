using SME.SERAp.Prova.Dominio;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioTipoProvaDeficiencia : IRepositorioBase<TipoProvaDeficiencia>
    {
        Task<bool> RemoverPorTipoProvaId(long tipoProvaId);
    }
}
