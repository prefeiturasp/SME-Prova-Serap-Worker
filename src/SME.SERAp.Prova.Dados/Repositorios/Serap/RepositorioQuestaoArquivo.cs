using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioQuestaoArquivo : RepositorioBase<QuestaoArquivo>, IRepositorioQuestaoArquivo
    {
        public RepositorioQuestaoArquivo(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<IEnumerable<QuestaoArquivo>> ObterArquivosPorProvaIdAsync(long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select *
                                from
	                                questao_arquivo
                                where
	                               questao_id in ( select q.id from questao q where q.prova_id = @provaId)";

                return await conn.QueryAsync<QuestaoArquivo>(query, new { provaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<long> ObterQuestaoArquivoIdPorQuestaoIdArquivoId(long questaoId, long arquivoId)
        {
            using var conn = ObterConexao();
            try
            {
                const string query = @"select id 
                                        from questao_arquivo 
                                        where questao_id = @questaoId 
                                          and arquivo_id = @arquivoId
                                          limit 1";
                
                return await conn.QueryFirstOrDefaultAsync<long>(query, new { questaoId, arquivoId });
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
	                                questao_arquivo
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

        public async Task<bool> RemoverPorProvaIdAsync(long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"delete
                                from
	                                questao_arquivo
                                where
	                               questao_id in ( select q.id from questao q inner join prova p on q.prova_id = p.id where p.id = @provaId) ";

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
