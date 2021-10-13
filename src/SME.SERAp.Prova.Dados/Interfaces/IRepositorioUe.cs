using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioUe : IRepositorioBase<Ue>
    {
        Task<IEnumerable<Ue>> ObterUesSgpPorDreCodigo(string dreCodigo);
        Task<Ue> ObterUePorCodigo(string ueCodigo);
    }
}