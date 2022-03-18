using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioGrupoSerapCoreSso : IRepositorioBase<GrupoSerapCoreSso>
    {
        Task<IEnumerable<GrupoSerapCoreSso>> ObterGruposSerapCoreSso();
    }
}
