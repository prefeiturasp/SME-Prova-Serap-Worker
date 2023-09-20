using System.Threading.Tasks;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioResultadoCicloSme
    {
        Task<ResultadoCicloSme> ObterResultadoCicloSme(string edicao, int areaConhecimentoId, int cicloId);
        Task<long> IncluirAsync(ResultadoCicloSme resultado);
        Task<long> AlterarAsync(ResultadoCicloSme resultado);        
    }
}