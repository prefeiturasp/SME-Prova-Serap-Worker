using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioArquivo : RepositorioBase<Arquivo>, IRepositorioArquivo
    {
        public RepositorioArquivo(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<bool> RemoverPorIdsAsync(long[] ids)
        {
            using var conn = ObterConexao();
            try
            {
                const string query = @"delete from arquivo a
                                        where a.id = any(@ids)
                                        and not exists (select 1 from alternativa_arquivo aa where aa.arquivo_id = a.id limit 1)
                                        and not exists (select 1 from questao_arquivo qa where qa.arquivo_id = a.id limit 1)";

                await conn.ExecuteAsync(query, new { ids });
                return true;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<long> ObterIdArquivoPorCaminho(string caminho)
        {
            using var conn = ObterConexao();
            try
            {
                const string query = "select id from arquivo where caminho = @caminho limit 1";
                return await conn.QueryFirstOrDefaultAsync<long>(query, new { caminho });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<long> ObterIdArquivoPorCaminhoLegadoId(string caminho, long legadoId)
        {
            using var conn = ObterConexao();
            try
            {
                const string query = "select id from arquivo where caminho = @caminho and legado_id = @legadoId limit 1";
                return await conn.QueryFirstOrDefaultAsync<long>(query, new { caminho, legadoId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
