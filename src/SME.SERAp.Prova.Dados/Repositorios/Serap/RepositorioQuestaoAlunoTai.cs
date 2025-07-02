using System.Threading.Tasks;
using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioQuestaoAlunoTai : RepositorioBase<QuestaoAlunoTai>, IRepositorioQuestaoAlunoTai
    {
        public RepositorioQuestaoAlunoTai(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<bool> RemoverQuestaoAlunoTaiPorProvaIdAsync(long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                const string query = @"DELETE FROM questao_aluno_tai WHERE id IN (
							                  SELECT qat.id 
                                              FROM questao_aluno_tai qat
                                              INNER JOIN questao q ON qat.questao_id = q.id
                                              WHERE q.prova_id = @provaId
							        )";

                await conn.ExecuteAsync(query, new { provaId });

                return true;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<bool> ExisteQuestaoAlunoTaiPorAlunoId(long alunoId)
        {
            using var conn = ObterConexao();
            try
            {
                const string query = @"SELECT CASE WHEN EXISTS ( SELECT 1 FROM questao_aluno_tai WHERE aluno_id = @alunoId) THEN 1 ELSE 0 END";

                return await conn.ExecuteScalarAsync<bool>(query, new { alunoId });                
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}