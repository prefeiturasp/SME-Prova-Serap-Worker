using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioQuestao : RepositorioBase<Questao>, IRepositorioQuestao
    {
        public RepositorioQuestao(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<Questao> ObterPorIdEProvaIdLegadoAsync(long id, long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select questao.* from questao inner join prova on 
                                    questao.prova_id = prova.id 
                                    where questao.questao_legado_id = @id and prova.prova_legado_id = @provaId";

                return await conn.QueryFirstOrDefaultAsync<Questao>(query, new { id, provaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<Questao> ObterPorIdLegadoAsync(long id)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select * from questao where questao_legado_id = @id";

                return await conn.QueryFirstOrDefaultAsync<Questao>(query, new { id });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<Questao>> ObterQuestoesComImagemNaoSincronizadas()
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select * from questao where texto_base like '%http%'";

                return await conn.QueryAsync<Questao>(query);
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
	                                questao
                                where
	                               prova_id = @provaId";

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
