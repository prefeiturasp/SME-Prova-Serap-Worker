using SME.SERAp.Prova.Dominio.Entidades.ProficienciaPsp;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados.Interfaces
{
    public interface IRepositorioResultadoCicloDre
    {
        Task<long> AlterarAsync(ResultadoCicloDre resultado);
        Task<long> IncluirAsync(ResultadoCicloDre resultado);
        Task<ResultadoCicloDre> ObterResultadoCicloDre(string edicao, int areaConhecimentoId, string dre_sigla, int cicloId);
    }
}
