using SME.SERAp.Prova.Dominio;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados.Interfaces
{
    public interface IRepositorioResultadoSme
    {
        Task<ResultadoSme> ObterResultadoSme(string edicao, long areaConhecimentoId, string anoEscolar);
        Task<long> IncluirAsync(ResultadoSme resultado);
        Task<long> AlterarAsync(ResultadoSme resultado);
    }
}
