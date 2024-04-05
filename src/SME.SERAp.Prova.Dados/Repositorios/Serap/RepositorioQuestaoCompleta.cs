using System.Collections.Generic;
using System.Text;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System.Threading.Tasks;
using Dapper;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioQuestaoCompleta : RepositorioBase<QuestaoCompleta>, IRepositorioQuestaoCompleta
    {
        public RepositorioQuestaoCompleta(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task IncluirOuUpdateAsync(QuestaoCompleta questaoCompleta)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"insert into questao_completa (id, questao_legado_id, json, ultima_atualizacao) values (@id, @questaoLegadoId, @json, @UltimaAtualizacao) on conflict (id) do update set questao_legado_id = @questaoLegadoId, json = @json, ultima_atualizacao = @UltimaAtualizacao;";
                await conn.ExecuteAsync(query, questaoCompleta);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<QuestaoCompleta>> ObterQuestoesCompletasPorProvaIdParaCacheAsync(long provaId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = new StringBuilder();
                // questão
                query.AppendLine(" select qc.id, qc.json, qc.ultima_atualizacao as UltimaAtualizacao, qc.questao_legado_id as QuestaoLegadoId ");
                query.AppendLine(" from questao q ");
                query.AppendLine(" join questao_completa qc on qc.id = q.id ");
                query.AppendLine(" join prova p on p.id = q.prova_id ");
                query.AppendLine(" where q.prova_id = @provaId;");

                return await SqlMapper.QueryAsync<QuestaoCompleta>(conn, query.ToString(), new { provaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<QuestaoCompleta> ObterQuestaoCompletaPorQuestaoIdAsync(long questaoId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = new StringBuilder();
                query.AppendLine(" select qc.id, qc.json, qc.ultima_atualizacao as UltimaAtualizacao, qc.questao_legado_id as QuestaoLegadoId ");
                query.AppendLine(" from questao_completa qc ");
                query.AppendLine(" where qc.id = @questaoId;");

                return await SqlMapper.QueryFirstOrDefaultAsync<QuestaoCompleta>(conn, query.ToString(), new { questaoId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<QuestaoCompleta>> ObterQuestoesCompletasLegadoPorProvaIdParaCacheAsync(long provaId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = new StringBuilder();
                query.AppendLine(" select qc.questao_legado_id as id, max(qc.json) as json ");
                query.AppendLine(" from questao q ");
                query.AppendLine(" join questao_completa qc on qc.id = q.id ");
                query.AppendLine(" join prova p on p.id = q.prova_id ");
                query.AppendLine(" where q.prova_id = @provaId ");
                query.AppendLine(" group by qc.questao_legado_id;");

                return await SqlMapper.QueryAsync<QuestaoCompleta>(conn, query.ToString(), new { provaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
