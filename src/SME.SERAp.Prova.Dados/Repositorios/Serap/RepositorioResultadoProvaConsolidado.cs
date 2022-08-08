using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioResultadoProvaConsolidado : RepositorioSerapBase, IRepositorioResultadoProvaConsolidado
    {
        public RepositorioResultadoProvaConsolidado(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {
            
        }
        public async Task<IEnumerable<ConsolidadoProvaRespostaDto>> ObterExtracaoProvaRespostaFuncao(long provaSerapId, string dreCodigoEol, string ueCodigoEol)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select 
	                            ProvaSerapId,
                                ProvaSerapEstudantesId,
	                            DreCodigoEol, 
	                            DreSigla,
	                            DreNome,
	                            UeCodigoEol,
	                            UeNome,
	                            TurmaAnoEscolar,
	                            TurmaAnoEscolarDescricao,
	                            TurmaCodigo,
	                            TurmaDescricao,
	                            AlunoCodigoEol,
	                            AlunoNome,
	                            AlunoSexo,
	                            AlunoDataNascimento,
	                            ProvaComponente,
	                            ProvaCaderno,
                                ProvaQuantidadeQuestoes,
	                            AlunoFrequencia,  
	                            QuestaoId, 
	                            QuestaoOrdem,
	                            resposta
                           from f_extracao_prova_resposta(@provaSerapId, @dreCodigoEol, @ueCodigoEol);";

                return await conn.QueryAsync<ConsolidadoProvaRespostaDto>(query, new { provaSerapId, dreCodigoEol, ueCodigoEol }, commandTimeout: 9000);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Extrair dados para CSV. ProvaId:{provaSerapId}, CodigoDre:{dreCodigoEol}, CodigoUe:{ueCodigoEol} -- Erro: {ex.Message}");
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<ConsolidadoProvaRespostaDto>> ObterExtracaoProvaRespostaQuery(long provaSerapId, string dreCodigoEol, string ueCodigoEol, string[] turmasCodigosEol = null)
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
                                rpc.prova_data_inicio DataInicio,
                                rpc.prova_data_entregue DataFim,
	                            rpc.questao_id as QuestaoId, 
	                            rpc.questao_ordem as QuestaoOrdem,
	                            rpc.resposta as Resposta
                              from resultado_prova_consolidado rpc 
                                 ";                

                string where = " where 1=1 ";
                where += @"     and rpc.prova_serap_id = @provaSerapId
                                and rpc.dre_codigo_eol = @dreCodigoEol
                                and rpc.ue_codigo_eol = @ueCodigoEol 
                                 ";

                if (turmasCodigosEol != null)
                    where += " and rpc.turma_codigo = any(@turmasCodigosEol) ";

                query += where;

                return await conn.QueryAsync<ConsolidadoProvaRespostaDto>(query, new { provaSerapId, dreCodigoEol, ueCodigoEol, turmasCodigosEol }, commandTimeout: 9000);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Extrair dados para CSV. ProvaId:{provaSerapId}, CodigoDre:{dreCodigoEol}, CodigoUe:{ueCodigoEol} -- Erro: {ex.Message}");
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<bool> VerificaResultadoExtracaoProvaExiste(long provaLegadoId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select 1 from resultado_prova_consolidado where prova_serap_id = @provaLegadoId limit 1;";
                return await conn.QueryFirstOrDefaultAsync<bool>(query, new { provaLegadoId }, commandTimeout: 9000);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task ExcluirResultadoProvaAlunoTurma(long provaLegadoId, long alunoCodigoEol, string turmaCodigo)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"delete from resultado_prova_consolidado 
                                    where prova_serap_id = @provaLegadoId
                                      and aluno_codigo_eol = @alunoCodigoEol
                                      and turma_codigo = @turmaCodigo";

                await conn.ExecuteAsync(query, new { provaLegadoId, alunoCodigoEol, turmaCodigo }, commandTimeout: 9000);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<string>> ObterTurmasResultadoProvaAluno(long provaLegadoId, long alunoCodigoEol)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select distinct turma_codigo 
                                         from resultado_prova_consolidado 
                                        where prova_serap_id = @provaLegadoId 
                                          and aluno_codigo_eol = @alunoCodigoEol";

                return await conn.QueryAsync<string>(query, new { provaLegadoId, alunoCodigoEol }, commandTimeout: 9000);
            }
            catch (Exception ex)
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
