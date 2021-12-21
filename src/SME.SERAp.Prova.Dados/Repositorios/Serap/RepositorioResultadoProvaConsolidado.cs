using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioResultadoProvaConsolidado : RepositorioSerapBase, IRepositorioResultadoProvaConsolidado
    {
        public RepositorioResultadoProvaConsolidado(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {
            
        }
        public async Task<IEnumerable<ConsolidadoProvaRespostaDto>> ObterExtracaoProvaResposta(long provaSerapId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select 
	                        rpc.prova_serap_id ProvaSerapId,
                            rpc.prova_serap_estudantes_id ProvaSerapEstudantesId,
	                        rpc.dre_codigo_eol DreCodigoEol, 
	                        rpc.dre_sigla DreSigla,
	                        rpc.dre_nome DreNome,
	                        rpc.ue_codigo_eol UeCodigoEol,
	                        rpc.ue_nome UeNome,
	                        rpc.turma_ano_escolar TurmaAnoEscolar,
	                        rpc.turma_ano_escolar_descricao TurmaAnoEscolarDescricao,
	                        rpc.turma_codigo TurmaCodigo,
	                        rpc.turma_descricao TurmaDescricao,
	                        rpc.aluno_codigo_eol AlunoCodigoEol,
	                        rpc.aluno_nome AlunoNome,
	                        rpc.aluno_sexo AlunoSexo,
	                        rpc.aluno_data_nascimento AlunoDataNascimento,
	                        rpc.prova_componente ProvaComponente,
	                        rpc.prova_caderno ProvaCaderno,
                            rpc.prova_quantidade_questoes as ProvaQuantidadeQuestoes,
	                        rpc.aluno_frequencia AlunoFrequencia,  
	                        rpc.questao_id as QuestaoId, 
	                        rpc.questao_ordem as QuestaoOrdem,
	                        rpc.resposta
                        from resultado_prova_consolidado rpc where rpc.prova_serap_id = @provaSerapId;";

                return await conn.QueryAsync<ConsolidadoProvaRespostaDto>(query, new { provaSerapId }, commandTimeout: 600);
            }
            catch (System.Exception ex)
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
