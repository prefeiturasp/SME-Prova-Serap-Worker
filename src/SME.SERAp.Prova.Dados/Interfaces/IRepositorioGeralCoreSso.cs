using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioGeralCoreSso
    {
        Task<IEnumerable<string>> ObterUeDreAtribuidasCoreSso(Guid usuarioIdCoreSso, Guid grupoIdCoreSso);
        Task<IEnumerable<UeDreAtribuidaCoreSsoDto>> ObterUeDreAtribuidasCoreSsoPorUsuarioIdAsync(Guid usuarioIdCoreSso);
    }
}
