using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Text;
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
                var query = @"";

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
