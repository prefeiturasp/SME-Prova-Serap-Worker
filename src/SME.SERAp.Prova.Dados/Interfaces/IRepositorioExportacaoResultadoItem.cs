using SME.SERAp.Prova.Dominio;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioExportacaoResultadoItem : IRepositorioBase<ExportacaoResultadoItem>
    {
        Task ExcluirExportacaoResultadoItemPorIdAsync(long Id);
        Task<bool> ConsultarSeExisteItemProcessoPorIdAsync(long IdProcesso);
        Task ExcluirItensPorProcessoIdAsync(long ProcessoId);
    }
}
