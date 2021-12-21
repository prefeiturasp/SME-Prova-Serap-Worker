using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioProva : IRepositorioBase<Dominio.Prova>
    {
        Task<Dominio.Prova> ObterPorIdLegadoAsync(long id);
        Task<bool> VerificaSeExisteRespostasPorId(long id);
        Task<bool> VerificaSeExistePorProvaSerapId(long provaId);
        Task CriarProvaRespostasExtracao(long provaId);
        Task ConsolidarProvaRespostasPorProvaSerapId(long provaId);
        Task LimparDadosConsolidadosPorProvaSerapId(long provaId);
        Task ConsolidarProvaRespostasPorFiltros(long provaId, string dreId, string[] ueIds);
        Task LimparDadosConsolidadosPorFiltros(long provaId, string dreId, string[] ueIds);
    }
}
