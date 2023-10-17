using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
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

        public async Task<QuestaoCompletaDto> MontarQuestaoCompletaPorIdAsync(long id)
        {
            using var conn = ObterConexaoLeitura();

            try
            {
                var query = new StringBuilder();

                // questão
                query.AppendLine(" select q.id, q.questao_legado_id as QuestaoLegadoId, q.texto_base as titulo, q.enunciado as descricao, q.ordem, q.tipo, q.quantidade_alternativas as quantidadeAlternativas ");
                query.AppendLine(" from questao q ");
                query.AppendLine(" where q.id = @id; ");

                // arquivos
                query.AppendLine(" select distinct ar.id, ar.legado_id as legadoId, q.id as questaoId, ar.caminho, ar.tamanho_bytes as tamanhoBytes");
                query.AppendLine(" from questao q ");
                query.AppendLine(" join questao_arquivo qa on qa.questao_id = q.id ");
                query.AppendLine(" join arquivo ar on ar.id = qa.arquivo_id ");
                query.AppendLine(" where q.id = @id; ");

                // arquivos audio
                query.AppendLine(" select distinct ar.id, ar.legado_id as legadoId, q.id as questaoId, ar.caminho, ar.tamanho_bytes as tamanhoBytes");
                query.AppendLine(" from questao q ");
                query.AppendLine(" join questao_audio qa on qa.questao_id = q.id ");
                query.AppendLine(" join arquivo ar on ar.id = qa.arquivo_id ");
                query.AppendLine(" where q.id = @id; ");

                // arquivos video
                query.AppendLine(" select distinct q.id as questaoId, qv.id, ");
                query.AppendLine("     ar.caminho, ar.tamanho_bytes as tamanhoBytes,  ");
                query.AppendLine("     art.caminho as caminhoVideoThumbinail, art.tamanho_bytes as videoThumbinailTamanhoBytes, ");
                query.AppendLine("     arc.caminho as caminhoVideoConvertido, arc.tamanho_bytes as videoConvertidoTamanhoBytes ");
                query.AppendLine(" from questao q ");
                query.AppendLine(" join questao_video qv on qv.questao_id = q.id ");
                query.AppendLine(" join arquivo ar on ar.id = qv.arquivo_video_id ");
                query.AppendLine(" left join arquivo art on art.id = qv.arquivo_thumbnail_id ");
                query.AppendLine(" left join arquivo arc on arc.id = qv.arquivo_video_convertido_id ");
                query.AppendLine(" where q.id = @id; ");

                // alternativas
                query.AppendLine(" select q.id as questaoId, a.id, a.alternativa_legado_id as alternativaLegadoId, a.descricao, a.ordem, a.numeracao ");
                query.AppendLine(" from questao q ");
                query.AppendLine(" join alternativa a on a.questao_id = q.id ");
                query.AppendLine(" where q.id = @id; ");

                // arquivos alternativas
                query.AppendLine(" select distinct ar.id, ar.legado_id as legadoId, a.questao_id as questaoId, ar.caminho, ar.tamanho_bytes as tamanhoBytes ");
                query.AppendLine(" from questao q ");
                query.AppendLine(" join alternativa a on a.questao_id = q.id ");
                query.AppendLine(" join alternativa_arquivo aa on aa.alternativa_id = a.id ");
                query.AppendLine(" join arquivo ar on ar.id = aa.arquivo_id ");
                query.AppendLine(" where q.id = @id; ");

                using var sqlMapper = await SqlMapper.QueryMultipleAsync(conn, query.ToString(), new { id }, commandTimeout: 300);
                var questaoCompleta = await sqlMapper.ReadFirstOrDefaultAsync<QuestaoCompletaDto>();

                if (questaoCompleta == null || questaoCompleta.Id <= 0)
                    return questaoCompleta;

                questaoCompleta.Arquivos = sqlMapper.Read<ArquivoDto>();
                questaoCompleta.Audios = sqlMapper.Read<ArquivoDto>();
                questaoCompleta.Videos = sqlMapper.Read<ArquivoVideoDto>();
                questaoCompleta.Alternativas = sqlMapper.Read<AlternativaDto>();

                var arquivosAlternativas = sqlMapper.Read<ArquivoDto>().ToList();

                if (!arquivosAlternativas.Any()) 
                    return questaoCompleta;
                
                var arquivosQuestao = questaoCompleta.Arquivos.ToList();
                arquivosQuestao.AddRange(arquivosAlternativas);
                questaoCompleta.Arquivos = arquivosQuestao;

                return questaoCompleta;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<QuestaoAtualizada>> ObterQuestoesAtualizadas(long provaId, int pagina, int quantidade)
        {
            var ignorarRegistros = ((pagina - 1) * quantidade);
            using var conn = ObterConexao();
            try
            {
                var query = @"select 
                                q.id,
                                qc.ultima_atualizacao as UltimaAtualizacaoQuestao 
                              from questao q
                              left join questao_completa qc on qc.id = q.id
                              where q.prova_id = @provaId
                              order by q.prova_id, q.id
                              limit @quantidade offset @ignorarRegistros";
                
                return await conn.QueryAsync<QuestaoAtualizada>(query, new { provaId, quantidade, ignorarRegistros });
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

        public async Task<long> ObterIdQuestaoPorProvaIdCadernoLegadoId(long provaId, string caderno, long questaoLegadoId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select id from questao where prova_id = @provaId and caderno = @caderno and questao_legado_id = @questaoLegadoId";
                return await conn.ExecuteScalarAsync<long>(query, new { provaId, caderno, questaoLegadoId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
