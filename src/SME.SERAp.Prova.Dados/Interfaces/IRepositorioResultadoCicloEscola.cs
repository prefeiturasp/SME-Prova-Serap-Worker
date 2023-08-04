using System.Threading.Tasks;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioResultadoCicloEscola
    {
        Task<ResultadoCicloEscola> ObterResultadoCicloEscola(string edicao, int areaConhecimentoId, string uadSigla,
            string escCodigo, int cicloId);
        Task<long> IncluirAsync(ResultadoCicloEscola resultado);
        Task<long> AlterarAsync(ResultadoCicloEscola resultado);        
    }
}