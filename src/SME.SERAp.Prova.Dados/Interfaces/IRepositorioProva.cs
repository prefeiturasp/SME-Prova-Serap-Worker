using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos;
using System;
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
        Task ConsolidarProvaRespostasPorProvaSerapId(long provaId);
        Task LimparDadosConsolidadosPorProvaSerapId(long provaId);
        Task ConsolidarProvaRespostasPorFiltros(long provaId, string dreId, string ueId, string turmaCodigo);
        Task<IEnumerable<ProvaBIBSyncDto>> ObterProvasBibAsync();
        Task LimparDadosConsolidadosPorFiltros(long provaId, string dreId, string ueId, string turmaCodigo);
        Task<IEnumerable<ResultadoProvaConsolidado>> ObterDadosPorUeId(long provaId, string dreId, string ueId);
        Task<bool> FinalizarProvaAsync(ProvaParaAtualizarDto provaParaAtualizar);
        Task<IEnumerable<ProvaAlunoDto>> ObterProvasIniciadasPorModalidadeAsync(int modalidade);
        Task<IEnumerable<ProvaTaiSyncDto>> ObterProvasTaiAsync();
        Task<IEnumerable<ProvaAtualizadaDto>> ObterProvaPorUltimaAtualizacao(DateTime dataBase);
        Task<bool> VerificaSePossuiDownload(long provaId);
        Task<IEnumerable<Dominio.Prova>> ObterProvasLiberadasNoPeriodoParaCacheAsync();        
    }
}
