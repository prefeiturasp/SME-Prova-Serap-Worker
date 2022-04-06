using SME.SERAp.Prova.Dominio;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioUsuarioGrupoSerapCoreSso : IRepositorioBase<UsuarioGrupoSerapCoreSso>
    {
        Task<UsuarioGrupoSerapCoreSso> ObterPorUsuarioIdEGrupoIdCoreSso(long usuarioId, long grupoId);
        Task<bool> RemoverPorUsuarioSerapIdEGrupoId(long usuarioSerapId, long grupoSerapId);
    }
}
