using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioProvaLegado
    {
        Task<IEnumerable<long>> ObterProvasIdsParaSeremSincronizadasIds(DateTime ultimaAtualizacao);
        Task<ProvaLegadoDetalhesIdDto> ObterDetalhesPorId(long id);
        Task<IEnumerable<long>> ObterAlternativasPorProvaIdEQuestaoId(long questaoId);
        Task<IEnumerable<QuestaoLegadoDto>> ObterQuestoesPorProvaId(long provaId);
        Task<QuestoesPorProvaIdDto> ObterDetalheQuestoesPorProvaId(long provaLegadoId, long questaoLegadoId);
        Task<AlternativasProvaIdDto> ObterDetalheAlternativasPorProvaIdEQuestaoId(long questaoId,
            long alternativaId);
        Task<IEnumerable<ContextoProvaLegadoDto>> ObterContextosProvaPorProvaId(long provaId);
    }
}