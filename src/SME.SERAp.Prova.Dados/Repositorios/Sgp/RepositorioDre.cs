using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioDre : RepositorioBase<Dre>, IRepositorioDre
    {
        public RepositorioDre(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<IEnumerable<Dre>> ObterDresSgp()
        {
            using var conn = ObterConexaoSgp();
            try
            {
                const string query = @"select * from dre";

                return await conn.QueryAsync<Dre>(query);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<Dre> ObterDREPorCodigo(string dreCodigo)
        {
            using var conn = ObterConexao();
            try
            {
                const string query = @"select * from dre where dre_id = @dreCodigo ";
                return await conn.QueryFirstOrDefaultAsync<Dre>(query, new { dreCodigo });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
