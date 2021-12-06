using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
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
                var query = @"delete from prova_ano_original where prova_id = @provaId";

                await conn.ExecuteAsync(query, new { provaId });

                return true;
            }
            catch (System.Exception ex)
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
