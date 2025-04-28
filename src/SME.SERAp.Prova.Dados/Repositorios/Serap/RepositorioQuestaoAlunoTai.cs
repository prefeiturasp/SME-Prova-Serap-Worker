using System.Collections.Generic;
using System.Threading.Tasks;
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
                const string query = @"delete from questao_aluno_tai 
                                        where questao_id in (select q.id 
                                                                from questao q 
                                                                where q.prova_id = @prova_id)";

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
    }
}