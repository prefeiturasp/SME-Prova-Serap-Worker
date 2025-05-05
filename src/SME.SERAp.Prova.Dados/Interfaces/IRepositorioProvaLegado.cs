using SME.SERAp.Prova.Dominio;
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
        Task<ProvaLegadoDetalhesIdDto> ObterProvaPorId(long id);
        Task<IEnumerable<long>> ObterAlternativasPorProvaIdEQuestaoId(long questaoId);
        Task<IEnumerable<QuestoesPorProvaIdDto>> ObterQuestoesPorProvaId(long provaId);
        Task<IEnumerable<QuestoesPorProvaIdDto>> ObterQuestoesPorProvaIdTEMP(long provaId);
        Task<QuestoesPorProvaIdDto> ObterDetalheQuestoesPorProvaId(long provaLegadoId, long questaoLegadoId);
        Task<AlternativasProvaIdDto> ObterDetalheAlternativasPorProvaIdEQuestaoId(long questaoId,
            long alternativaId);
        Task<IEnumerable<ContextoProvaLegadoDto>> ObterContextosProvaPorProvaId(long provaId);
        Task<IEnumerable<Arquivo>> ObterAudiosPorQuestaoId(long questaoId);
        Task<IEnumerable<QuestaoVideoDto>> ObterVideosPorQuestaoId(long questaoId);
        Task<IEnumerable<ProvaAnoDetalheDto>> ObterProvaAnoDetalhesPorId(long id);
        Task<IEnumerable<AmostraProvaTaiDto>> ObterDadosAmostraProvaTai(long provaId);
        Task<IEnumerable<ProvaGrupoPermissaoDto>> ObterDadosProvaGrupoPermissaoPorId(long provaId);
    }
}