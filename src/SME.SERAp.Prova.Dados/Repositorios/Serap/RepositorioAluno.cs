using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioAluno : RepositorioBase<Aluno>, IRepositorioAluno
    {
        public RepositorioAluno(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<Aluno> ObterAlunoPorCodigo(long codigo)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select id, nome, turma_id as TurmaId, ra from aluno where ra = @codigo";

                return await conn.QueryFirstOrDefaultAsync<Aluno>(query, new { codigo });
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

        public async Task<IEnumerable<Aluno>> ObterAlunosPorTurmaIdAsync(long turmaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select id, nome, turma_id as TurmaId, ra from aluno where turma_id = @turmaId";

                return await conn.QueryAsync<Aluno>(query, new { turmaId });
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
