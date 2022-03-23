using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioGeralCoreSso
    {
        Task<IEnumerable<string>> ObterUeDreAtribuidasCoreSso(Guid usuarioIdCoreSso, Guid grupoIdCoreSso);
    }
}
