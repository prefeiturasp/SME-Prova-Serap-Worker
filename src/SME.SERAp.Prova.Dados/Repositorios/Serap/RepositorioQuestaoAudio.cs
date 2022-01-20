using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioQuestaoAudio : RepositorioBase<QuestaoAudio>, IRepositorioQuestaoAudio
    {
        public RepositorioQuestaoAudio(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<IEnumerable<QuestaoAudio>> ObterPorQuestaoId(long questaoId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select id, 
                                    questao_id QuestaoId, 
                                    arquivo_id ArquivoId  
                                from questao_audio 
                                    where questao_id = @questaoId;";

                return await conn.QueryAsync<QuestaoAudio>(query, new { questaoId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<QuestaoAudio>> ObterPorProvaId(long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select qa.id, qa.questao_id QuestaoId, qa.arquivo_id ArquivoId
                                from questao q
                                inner join questao_audio qa on q.id = qa.questao_id
                                inner join arquivo a on qa.arquivo_id = a.id
                                where q.prova_id = @provaId";

                return await conn.QueryAsync<QuestaoAudio>(query, new { provaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<bool> RemoverPorIdsAsync(long[] ids)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"delete
                                from
	                                questao_audio
                                where
	                               id = any(@ids)";

                await conn.ExecuteAsync(query, new { ids });
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
