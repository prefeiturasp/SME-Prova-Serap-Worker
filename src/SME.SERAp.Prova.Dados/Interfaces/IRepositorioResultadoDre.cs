using SME.SERAp.Prova.Dominio;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioResultadoDre : IRepositorioProvaSpBase
    {
        Task<ResultadoDre> ObterResultadoDre(string edicao, long areaConhecimentoId, string uad_sigla, string anoEscolar);
        Task<long> IncluirAsync(ResultadoDre resultado);
        Task<long> AlterarAsync(ResultadoDre resultado);
    }
}
