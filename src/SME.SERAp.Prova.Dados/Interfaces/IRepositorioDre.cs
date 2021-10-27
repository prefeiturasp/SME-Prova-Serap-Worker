using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioDre : IRepositorioBase<Dre>
    {
        Task<IEnumerable<Dre>> ObterDresSgp();
        Task<Dre> ObterDREPorCodigo(string dreCodigo);
    }
}