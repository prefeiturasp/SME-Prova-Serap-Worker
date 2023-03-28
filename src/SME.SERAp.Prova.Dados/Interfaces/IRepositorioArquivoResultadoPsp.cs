using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Infra;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados.Interfaces
{
    public interface IRepositorioArquivoResultadoPsp
    {
        public Task<ArquivoResultadoPspDto> ObterArquivoResultadoPspPorId(long id);

        Task AtualizarStatusArquivoResultadoPspPorId(long id, StatusImportacao state);

        Task FinalizarProcessosPorTipo(TipoResultadoPsp tipoResultadoProcesso);
    }
}
