using SME.SERAp.Prova.Dominio;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioResultadoTurma : IRepositorioProvaSpBase
    {
        Task<ResultadoTurma> ObterResultadoTurma(string edicao, long areaConhecimentoId, string esc_codigo, string tur_codigo);
        Task<long> IncluirAsync(ResultadoTurma resultado);
        Task<long> AlterarAsync(ResultadoTurma resultado);
    }
}
