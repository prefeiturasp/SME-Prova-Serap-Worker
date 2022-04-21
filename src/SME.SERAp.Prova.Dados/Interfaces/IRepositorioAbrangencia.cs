using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioAbrangencia : IRepositorioBase<Abrangencia>
    {
        Task<Abrangencia> ObterPorObjetoAbrangencia(Abrangencia abrangencia);
        Task<IEnumerable<Abrangencia>> ObterPorGrupoId(long grupoId);
        Task<bool> ExcluirPorId(long id);
        Task<Abrangencia> ObterPorUsuarioGrupoDreUeTurmaAsync(long usuarioId, long grupoId, long dreId, long ueId, long turmaId);
    }
}
