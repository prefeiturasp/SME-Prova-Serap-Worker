using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioCadernoAluno : RepositorioBase<CadernoAluno>, IRepositorioCadernoAluno
    {
        public RepositorioCadernoAluno(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<bool> RemoverCadernosPorProvaIdAsync(long provaId)
        {

            using var conn = ObterConexao();
            try
            {
                var query = @"delete  from caderno_aluno where prova_id = @provaId";

                await conn.ExecuteAsync(query, new { provaId });

                return true;
            }
            catch (System.Exception)
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
