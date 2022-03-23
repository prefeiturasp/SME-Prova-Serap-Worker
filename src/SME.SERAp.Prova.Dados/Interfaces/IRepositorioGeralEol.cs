using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioGeralEol
    {
        Task<IEnumerable<string>> ObterUeDreAtribuidasEolAsync(string codigoRf, string tiposEscola);
    }
}
