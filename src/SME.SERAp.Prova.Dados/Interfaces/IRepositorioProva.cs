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
        Task<long?> ObterProvaOrigemCadernoAsync(long provaId);
        Task CriarProvaRespostasExtracao(long provaId);
        Task<IEnumerable<ProvaBIBSyncDto>> ObterProvasBibAsync();
        Task<IEnumerable<ResultadoProvaConsolidado>> ObterDadosPorUeId(long provaId, string dreId, string ueId);
        Task<bool> FinalizarProvaAsync(ProvaParaAtualizarDto provaParaAtualizar);
        Task<IEnumerable<ProvaAlunoDto>> ObterProvasIniciadasPorModalidadeAsync(int modalidade);
        Task<int> ExcluirDadosConsolidadoPaginado(long provaLegadoId, int take, int skip);
        Task<int> ConsolidarDadosPaginado(long provaLegadoId, bool aderirTodos, bool paraEstudanteComDeficiencia, int take, int skip);
    }
}
