using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioExportacaoResultadoItem : RepositorioBase<ExportacaoResultadoItem>, IRepositorioExportacaoResultadoItem
    {
        public RepositorioExportacaoResultadoItem(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task ExcluirExportacaoResultadoItemPorIdAsync(long id)
        {
            using var conn = ObterConexao();
            try
            {
                var query = $@"delete from exportacao_resultado_item where id = @id;";
                await conn.ExecuteAsync(query, new { Id = id }, commandTimeout: 5000);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task ExcluirItensPorProcessoIdAsync(long processoId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = $@"delete from exportacao_resultado_item where exportacao_resultado_id = @ProcessoId;";
                await conn.ExecuteAsync(query, new { ProcessoId = processoId }, commandTimeout: 5000);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<ExportacaoResultadoItem> ObterExportacaoResultadoItemPorProcessoIdDreCodigo(long processoId, string dreCodigo)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                const string query = "select * from exportacao_resultado_item where exportacao_resultado_id = @processoId and dre_codigo_eol = @dreCodigo";
                return await conn.QueryFirstOrDefaultAsync<ExportacaoResultadoItem>(query, new { processoId, dreCodigo });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<bool> ConsultarSeExisteItemProcessoPorIdAsync(long idProcesso)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                const string query = "select 1 from exportacao_resultado_item where exportacao_resultado_id = @id;";
                var result = await conn.QueryFirstOrDefaultAsync(query, new { Id = idProcesso }, commandTimeout: 600);
                return result != null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
