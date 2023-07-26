using System.Threading.Tasks;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioResultadoCicloTurma
    {
        Task<ResultadoCicloTurma> ObterResultadoCicloTurma(string edicao, int areaConhecimentoId, string uadSigla,
            string escCodigo, string turmaCodigo);
        Task<long> IncluirAsync(ResultadoCicloTurma resultado);
        Task<long> AlterarAsync(ResultadoCicloTurma resultado);        
    }
}