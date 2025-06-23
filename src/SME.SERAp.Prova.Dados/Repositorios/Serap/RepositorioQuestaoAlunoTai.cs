using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.Dtos;
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
									    INNER JOIN aluno a ON qat.aluno_id = a.id
									    INNER JOIN prova_aluno pa ON a.ra = pa.aluno_ra
									    WHERE pa.prova_id = @provaId
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

        public async Task<IEnumerable<QuestaoAlunoTai>> ObterQuestoesAlunoTaiPorAlunoRaProvaId(long alunoRa, long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select qat.* from questao_aluno_tai qat 
                                inner join aluno a on a.id = qat.aluno_id
                                inner join questao q on q.id = qat.questao_id
                                where a.ra = @alunoRa and q.prova_id = @provaId";

                return await conn.QueryAsync<QuestaoAlunoTai>(query, new { alunoRa, provaId });
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