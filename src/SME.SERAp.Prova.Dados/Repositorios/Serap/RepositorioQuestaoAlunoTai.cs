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

        public async Task<int> ExcluirQuestaoAlunoTai(long provaId, long alunoRa)
        {
            using var conn = ObterConexao();
            try
            {
                const string query = @"delete
                                        from
	                                        questao_aluno_tai qat
                                        where
	                                        qat.id in(select qat2.id from questao_aluno_tai qat2
                                        inner join aluno a on a.id = qat2.aluno_id
                                        inner join questao q on q.id = qat2.questao_id
                                        where a.ra = @alunoRa and q.prova_id = @provaId)";

                return await conn.ExecuteAsync(query, new { provaId, alunoRa }); ;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}