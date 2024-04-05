using SME.SERAp.Prova.Dominio;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioExportacaoResultadoItem : IRepositorioBase<ExportacaoResultadoItem>
    {
        Task ExcluirExportacaoResultadoItemPorIdAsync(long id);
        Task<bool> ConsultarSeExisteItemProcessoPorIdAsync(long idProcesso);
        Task ExcluirItensPorProcessoIdAsync(long processoId);
        Task<ExportacaoResultadoItem> ObterExportacaoResultadoItemPorProcessoIdDreCodigo(long processoId, string dreCodigo);
    }
}
