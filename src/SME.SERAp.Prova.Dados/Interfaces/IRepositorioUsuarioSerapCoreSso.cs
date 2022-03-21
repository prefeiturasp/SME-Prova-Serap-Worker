using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioUsuarioSerapCoreSso : IRepositorioBase<UsuarioSerapCoreSso>
    {
        Task<UsuarioSerapCoreSso> ObterPorIdCoreSso(Guid idCoreSso);
        Task<UsuarioSerapCoreSso> ObterPorId(long id);
        Task<IEnumerable<UsuarioSerapCoreSso>> ObterPorIdGrupoSerap(long idGrupo);
    }
}
