using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioProvaAno : RepositorioBase<ProvaAno>, IRepositorioProvaAno
    {
        public RepositorioProvaAno(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<bool> RemoverAnosPorProvaIdAsync(long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                const string query = "delete from prova_ano where prova_id = @provaId";
                await conn.ExecuteAsync(query, new { provaId });

                return true;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<ProvaAnoDto>> ObterProvasAnosDatasEModalidadesParaCacheAsync()
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                const string query = @"select
                                            p.descricao,
                                            p.Id,
                                            p.total_Itens totalItens,
                                            p.inicio_download as InicioDownload,
                                            p.inicio,
                                            p.fim,
                                            p.Tempo_Execucao TempoExecucao,
                                            case when pa.modalidade is not null then pa.modalidade else p.modalidade end Modalidade,
                                            p.Senha,
                                            p.possui_bib PossuiBIB,
                                            pa.ano,
                                            pa.etapa_eja EtapaEja,
                                            p.qtd_itens_sincronizacao_respostas as  quantidadeRespostaSincronizacao,
                                            p.ultima_atualizacao as UltimaAtualizacao,
                                            tp.para_estudante_com_deficiencia as deficiente,
                                            p.prova_com_proficiencia ProvaComProficiencia,
                                            p.apresentar_resultados ApresentarResultados,
                                            p.apresentar_resultados_por_item ApresentarResultadosPorItem,
                                            p.formato_tai FormatoTai,
                                            p.formato_tai_item FormatoTaiItem,
                                            p.formato_tai_avancar_sem_responder FormatoTaiAvancarSemResponder,
                                            p.formato_tai_voltar_item_anterior FormatoTaiVoltarItemAnterior,
                                            p.exibir_video as ExibirVideo,
                                            p.exibir_audio as ExibirAudio
                                          from prova p
                                          inner join prova_ano pa on pa.prova_id = p.id
                                          inner join tipo_prova tp on tp.id = p.tipo_prova_id 
                                          where (p.ocultar_prova = false or ocultar_prova is null)
                                            and (aderir_todos or aderir_todos is null)";

                return await conn.QueryAsync<ProvaAnoDto>(query);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<TipoCurriculoPeriodoAnoDto>> ObterProvaAnoPorTipoCurriculoPeriodoId(int[] tcpIds)
        {
            using var conn = ObterConexao();
            try
            {
                const string query = @"select tcp_id as tcpId, ano, modalidade_codigo as Modalidade,etapa_eja as EtapaEja
                                        from tipo_curriculo_periodo_ano tcpa 
                                        where tcpa.tcp_id = ANY(@tcpIds)";

                return await conn.QueryAsync<TipoCurriculoPeriodoAnoDto>(query, new { tcpIds });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
