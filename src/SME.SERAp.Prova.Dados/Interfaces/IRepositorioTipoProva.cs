using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioTipoProva : IRepositorioBase<TipoProva>
    {
        Task<TipoProva> ObterPorLegadoId(long legadoId);
    }
}
