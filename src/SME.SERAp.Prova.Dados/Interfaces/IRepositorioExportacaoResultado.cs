using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioExportacaoResultado : IRepositorioBase<ExportacaoResultado>
    {
        Task<long> ObterStatusPorIdAsync(long id);
    }
}