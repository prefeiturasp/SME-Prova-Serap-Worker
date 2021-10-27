using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
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
                var query = @"select * from dre ";

                return await conn.QueryAsync<Dre>(query);
            }
            catch (Exception)
            {
                throw;
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
                var query = @"select * from dre where dre_id = @dreCodigo ";

                return await conn.QueryFirstOrDefaultAsync<Dre>(query, new { dreCodigo });
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
