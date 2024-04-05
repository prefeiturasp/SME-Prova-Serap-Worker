using System.Threading.Tasks;
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
    }
}