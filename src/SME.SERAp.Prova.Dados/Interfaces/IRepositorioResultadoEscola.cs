using SME.SERAp.Prova.Dominio;
using System.Threading.Tasks;


namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioResultadoEscola : IRepositorioProvaSpBase
    {
        Task<ResultadoEscola> ObterResultadoEscola(string edicao, long areaConhecimentoId, string esc_codigo, string anoEscolar);
        Task<long> IncluirAsync(ResultadoEscola resultado);
        Task<long> AlterarAsync(ResultadoEscola resultado);
    }
}
