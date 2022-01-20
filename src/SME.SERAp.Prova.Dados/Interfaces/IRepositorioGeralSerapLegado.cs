using SME.SERAp.Prova.Dominio;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioGeralSerapLegado
    {
        Task<TipoProva> ObterTipoProvaLegadoPorId(long tipoProvaLegadoId);
    }
}
