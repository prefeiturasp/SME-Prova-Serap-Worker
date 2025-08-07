using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioQuestaoAlunoResposta : RepositorioBase<QuestaoAlunoResposta>, IRepositorioQuestaoAlunoResposta
    {
        public RepositorioQuestaoAlunoResposta(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<QuestaoAlunoResposta> ObterPorIdRaAsync(long questaoId, long alunoRa)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select * from questao_aluno_resposta 
                        where questao_id = @questaoId and 
                        aluno_ra = @alunoRa";

                return await conn.QueryFirstOrDefaultAsync<QuestaoAlunoResposta>(query, new { questaoId, alunoRa });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<int> ExcluirRespostaAluno(long provaId, long alunoRa)
        {
            using var conn = ObterConexao();
            try
            {
                var query = $@"delete
                                from
	                                questao_aluno_resposta qar
                                where
	                                qar.id in (
	                                select
		                                qar2.id
	                                from
		                                questao_aluno_resposta qar2
	                                inner join questao q on
		                                q.id = qar2.questao_id
	                                where
		                                qar2.aluno_ra = @alunoRa
		                                and q.prova_id = @provaId)";
                return await conn.ExecuteAsync(query, new { provaId, alunoRa });
            }
            catch (Exception ex)
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
