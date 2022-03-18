using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioUsuarioCoreSso
    {
        Task<IEnumerable<UsuarioCoreSsoDto>> ObterUsuariosPorGrupoCoreSso(Guid grupo);
    }
}
