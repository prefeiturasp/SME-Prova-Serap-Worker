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
                var query = @"delete
                                from
	                                arquivo
                                where
	                               id = any(@ids)";

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
    }
}
