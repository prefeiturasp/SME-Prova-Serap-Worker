using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados.Interfaces
{
    public interface IRepositorioProvaGrupoPermissao : IRepositorioBase<ProvaGrupoPermissao>
    {
        Task<IEnumerable<ProvaGrupoPermissao>> ObterPorProvaIdAsync(long ProvaId);
    }
}
