using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioUe : RepositorioBase<Ue>, IRepositorioUe
    {
        public RepositorioUe(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }
        public async Task<IEnumerable<Ue>> ObterUesSgpPorDreCodigo(string dreCodigo)
        {
            using var conn = ObterConexaoSgp();
            try
            {
                var query = @"select ue.* 
                                from ue
                               inner join dre on dre.id = ue.dre_id 
                               where dre.dre_id = @dreCodigo ";

                return await conn.QueryAsync<Ue>(query, new { dreCodigo });
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

        public async Task<Ue> ObterUePorCodigo(string ueCodigo)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select * from ue where ue_id = @ueCodigo ";

                return await conn.QueryFirstOrDefaultAsync<Ue>(query, new { ueCodigo });
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
