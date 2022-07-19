using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System.Threading.Tasks;

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
    }
}
