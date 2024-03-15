using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos;
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
                where += @"     and rpc.prova_serap_id = @provaSerapId";


                if (dreCodigoEol != null)
                    where += " and rpc.dre_codigo_eol = @dreCodigoEol ";

                if (ueCodigoEol != null)
                    where += "and rpc.ue_codigo_eol = @ueCodigoEol ";

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

        public async Task ExcluirDadosConsolidadosPorProvaSerapEstudantesId(long provaSerapEstudantesId)
        {
            using var conn = ObterConexaoLeitura(); 
            try
            {
                var query = $@"delete from resultado_prova_consolidado where prova_serap_estudantes_id = @provaSerapEstudantesId;";
                await conn.ExecuteAsync(query, new { provaSerapEstudantesId }, commandTimeout: 50000);
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


        public async Task ExcluirDadosConsolidadosPorProvaLegadoId(long provaSerapId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = $@"delete from resultado_prova_consolidado where prova_serap_id = @provaSerapId;";
                await conn.ExecuteAsync(query, new { provaSerapId }, commandTimeout: 50000);
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


        public async Task ObterRespostasAlunoPorProvaIdEAlunoCodigoEol(long provaId, long alunoCodigoEol)
        {
            using var conn = ObterConexaoLeitura();
            try
            {

                var query = @" select q.id as questao_id, 
                                             q.ordem  as questao_ordem,
                                             qar.resposta
                                      from  questao   q                                                                                     
                                      left join questao_aluno_resposta qar on qar.questao_id  = q.id  and  qar.aluno_ra  = @alunoCodigoEol
                                      left join alternativa alt on alt.id = qar.alternativa_id
                                      left join alternativa alt2 on alt2.questao_id = q.id and alt2.correta 
                                      WHERE  q.prova_id = @provaId
                                      order by q.ordem";

                await conn.QueryAsync<string>(query, new { provaId, alunoCodigoEol }, commandTimeout: 9000);
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

    
        public async Task IncluirResultadoProvaConsolidado(ResultadoProvaConsolidado resultado)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                string query = @"INSERT INTO public.resultado_prova_consolidado
                                (prova_serap_id, 
                                 prova_serap_estudantes_id, 
                                 dre_codigo_eol,
                                 dre_sigla, 
                                 dre_nome, 
                                 ue_codigo_eol, 
                                 ue_nome,
                                 turma_ano_escolar,
                                 turma_ano_escolar_descricao, 
                                 turma_codigo, 
                                 turma_descricao,
                                 aluno_codigo_eol, 
                                 aluno_nome,
                                 aluno_sexo,
                                 aluno_data_nascimento,
                                 prova_componente,
                                 prova_caderno,
                                 prova_quantidade_questoes, 
                                 aluno_frequencia, 
                                 questao_id, 
                                 questao_ordem,
                                 resposta, 
                                 prova_data_inicio, 
                                 prova_data_entregue)
                                VALUES(@provaSerapId, 
                                       @provaSerapEstudantesId, 
                                       @DreCodigoEol, 
                                       @DreSigla, 
                                       @DreNome, 
                                       @UeCodigoEol, 
                                       @UeNome, 
                                       @TurmaAnoEscolar, 
                                       @TurmaAnoEscolarDescricao, 
                                       @TurmaCodigo,
                                       @TurmaDescricao, 
                                       @AlunoCodigoEol, 
                                       @AlunoNome,
                                       @AlunoSexo, 
                                       @AlunoDataNascimento,
                                       @ProvaComponente,
                                       @ProvaCaderno,
                                       @ProvaQuantidadeQuestoes,
                                       @AlunoFrequencia,
                                       @QuestaoId, 
                                       @QuestaoOrdem, 
                                       @Resposta, 
                                       @DataInicio,
                                       @DataFim);
                                ";

                await conn.ExecuteAsync(query, resultado);

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

        public async Task<IEnumerable<AlunoQuestaoRespostasDto>> ObterQuestaoAlunoRespostaPorProvaLegadoIdEAlunoRA(long provaLegadoId, long alunoRa)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                const string query = @"select qar.questao_id as QuestaoId, 
                                            q.ordem as QuestaoOrdem,
                                            CASE
                                                WHEN qar.alternativa_id IS NOT NULL THEN alt.numeracao
                                                ELSE qar.resposta
                                            END AS resposta
                                        from questao_aluno_resposta qar
                                        join questao q on q.id = qar.questao_id
                                        inner join prova p on p.id  = q.prova_id
                                        LEFT JOIN alternativa alt on alt.questao_id = qar.questao_id
                                            and alt.id = qar.alternativa_id
                                        WHERE qar.id in (select max(qar2.id) from questao_aluno_resposta qar2 where qar2.questao_id = qar.questao_id and qar2.aluno_ra = qar.aluno_ra)
                                        AND p.prova_legado_id  = @provaLegadoId
                                        AND qar.aluno_ra = @alunoRa
                                        order by q.ordem";

                return await conn.QueryAsync<AlunoQuestaoRespostasDto>(query, new { provaLegadoId, alunoRa });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
