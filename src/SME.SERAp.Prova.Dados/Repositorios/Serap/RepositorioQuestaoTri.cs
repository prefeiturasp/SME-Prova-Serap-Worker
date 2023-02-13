using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioQuestaoTri : RepositorioBase<QuestaoTri>, IRepositorioQuestaoTri
    {
        public RepositorioQuestaoTri(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<QuestaoTri> ObterPorQuestaoIdAsync(long questaoId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select id, 
                                     questao_id as QuestaoId, 
                                     discriminacao as Discriminacao, 
                                     dificuldade as Dificuldade, 
                                     acerto_casual as AcertoCasual
                                from questao_tri 
                                where questao_id = @questaoId";

                return await conn.QueryFirstOrDefaultAsync<QuestaoTri>(query, new { questaoId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
