using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioProva : IRepositorioBase<Dominio.Prova>
    {
        Task<Dominio.Prova> ObterPorIdLegadoAsync(long id);
        Task<bool> VerificaSeExisteRespostasPorId(long id);
        Task<bool> VerificaSeExisteProvaFinalizadaPorId(long id);
        Task<bool> VerificaSeExistePorProvaSerapId(long provaId);
        Task CriarProvaRespostasExtracao(long provaId);
        Task ConsolidarProvaRespostasPorProvaSerapId(long provaId);
        Task LimparDadosConsolidadosPorProvaSerapId(long provaId);
        Task ConsolidarProvaRespostasPorFiltros(long provaId, string dreId, string ueId);
        Task LimparDadosConsolidadosPorFiltros(long provaId, string dreId, string ueId);
        Task<IEnumerable<ResultadoProvaConsolidado>> ObterDadosPorUeId(long provaId, string dreId, string ueId);
        Task<bool> FinalizarProvaAsync(ProvaParaAtualizarDto provaParaAtualizar);
        Task<IEnumerable<ProvaAlunoDto>> ObterProvasIniciadasPorModalidadeAsync(int modalidade);
    }
}
