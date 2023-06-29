using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioProva : RepositorioBase<Dominio.Prova>, IRepositorioProva
    {
        public RepositorioProva(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }        

        public async Task<IEnumerable<ResultadoProvaConsolidado>> ObterDadosPorUeId(long provaId, string dreId, string ueId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {

                var query = $@"                            
                            select 
	                        vape.prova_serap_id,
                            vape.prova_serap_estudantes_id,
	                        vape.dre_codigo_eol, 
	                        vape.dre_sigla,
	                        vape.dre_nome,
	                        vape.ue_codigo_eol,
	                        vape.ue_nome,
	                        vape.turma_ano_escolar,
	                        vape.turma_ano_escolar_descricao,
	                        vape.turma_codigo,
	                        vape.turma_descricao,
	                        vape.aluno_codigo_eol,
	                        vape.aluno_nome,
	                        vape.aluno_sexo,
	                        vape.aluno_data_nascimento,
	                        vape.prova_componente,
	                        vape.prova_caderno,
                            vape.prova_quantidade_questoes,
	                        vape.aluno_frequencia,  
	                        q.id as questao_id, 
	                        q.ordem + 1 as questao_ordem,
	                        case
		                        when qar.alternativa_id is not null then a.numeracao
		                        else qar.resposta
	                        end as resposta
                        from v_aluno_prova_extracao vape 
                        inner join questao q on vape.prova_serap_estudantes_id = q.prova_id and vape.prova_caderno = q.caderno 
                        left join questao_aluno_resposta qar on q.id = qar.questao_id and vape.aluno_codigo_eol = qar.aluno_ra
                        left join alternativa a on qar.alternativa_id = a.id
                        where vape.prova_serap_id = @provaId 
                        and vape.dre_codigo_eol = @dreId 
                        and vape.ue_codigo_eol = @ueId;";

                return await conn.QueryAsync<ResultadoProvaConsolidado>(query, new { provaId, dreId, ueId }, commandTimeout: 50000);
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

        public async Task<int> ExcluirDadosConsolidadoPaginado(long provaLegadoId, int take, int skip)
        {
            using var conn = ObterConexao();
            try
            {
                var query = $@"WITH tb AS (
                                    select prova_serap_id, aluno_codigo_eol  
                                        from resultado_prova_consolidado r
                                        where r.prova_serap_id = @provaLegadoId
                                    limit @take 
                                    offset @skip
                                    )
                                DELETE FROM resultado_prova_consolidado r
                                    USING tb WHERE r.prova_serap_id = tb.prova_serap_id
                                        and r.aluno_codigo_eol = tb.aluno_codigo_eol;";

                return await conn.ExecuteAsync(query, new { provaLegadoId, take, skip }, commandTimeout: 50000);
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

        public async Task<int> ConsolidarDadosPaginado(long provaLegadoId, bool aderirTodos, bool paraEstudanteComDeficiencia, int take, int skip)
        {
            using var conn = ObterConexao();
            try
            {
                var queryDados = aderirTodos ? QueryAderirTodos(paraEstudanteComDeficiencia) : QueryAdesao(paraEstudanteComDeficiencia);
                var query = QueryBaseConsolidarDados(queryDados);
                return await conn.ExecuteAsync(query, new { provaLegadoId, take, skip }, commandTimeout: 50000);
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

        private string QueryBaseConsolidarDados(string queryDados)
        {
            return $@"	insert into resultado_prova_consolidado 
							(prova_serap_id,prova_serap_estudantes_id,dre_codigo_eol,dre_sigla,dre_nome,
							ue_codigo_eol,ue_nome,turma_ano_escolar,turma_ano_escolar_descricao,
							turma_codigo,turma_descricao,aluno_codigo_eol,aluno_nome,aluno_sexo,
							aluno_data_nascimento,prova_componente,prova_caderno,prova_quantidade_questoes,
							aluno_frequencia,questao_id,questao_ordem,resposta,prova_data_inicio,prova_data_entregue)

                        SELECT DISTINCT tb.prova_serap_id,
											tb.prova_serap_estudantes_id,
											d.dre_id AS dre_codigo_eol,
											d.abreviacao AS dre_sigla,
											d.nome AS dre_nome,
											u.ue_id AS ue_codigo_eol,
											(CASE
												 WHEN u.tipo_escola = 1 THEN 'EMEF'::text
												 WHEN u.tipo_escola = 2 THEN 'EMEI'::text
												 WHEN u.tipo_escola = 3 THEN 'EMEFM'::text
												 WHEN u.tipo_escola = 4 THEN 'EMEBS'::text
												 WHEN u.tipo_escola = 10 THEN 'CEI DIRET'::text
												 WHEN u.tipo_escola = 11 THEN 'CEI INDIR'::text
												 WHEN u.tipo_escola = 12 THEN 'CR.P.CONV'::text
												 WHEN u.tipo_escola = 13 THEN 'CIEJA'::text
												 WHEN u.tipo_escola = 14 THEN 'CCI/CIPS'::text
												 WHEN u.tipo_escola = 15 THEN 'ESC.PART.'::text
												 WHEN u.tipo_escola = 16 THEN 'CEU EMEF'::text
												 WHEN u.tipo_escola = 17 THEN 'CEU EMEI'::text
												 WHEN u.tipo_escola = 18 THEN 'CEU CEI'::text
												 WHEN u.tipo_escola = 19 THEN 'CEU'::text
												 WHEN u.tipo_escola = 22 THEN 'MOVA'::text
												 WHEN u.tipo_escola = 23 THEN 'CMCT'::text
												 WHEN u.tipo_escola = 25 THEN 'E TEC'::text
												 WHEN u.tipo_escola = 26 THEN 'ESP CONV'::text
												 WHEN u.tipo_escola = 27 THEN 'CEU AT COMPL'::text
												 WHEN u.tipo_escola = 29 THEN 'CCA'::text
												 WHEN u.tipo_escola = 28 THEN 'CEMEI'::text
												 WHEN u.tipo_escola = 30 THEN 'CECI'::text
												 WHEN u.tipo_escola = 31 THEN 'CEU CEMEI'::text
												 ELSE NULL::text
											 END || ' '::text) || u.nome::text AS ue_nome,
											t.ano AS turma_ano_escolar,
											CASE
												WHEN t.ano::text <> 'S'::text THEN (t.ano::text || 'ano'::text)::CHARACTER varying
												ELSE t.ano
											END AS turma_ano_escolar_descricao,
											t.codigo AS turma_codigo,
											t.nome AS turma_descricao,
											tb.aluno_codigo_eol,
											tb.aluno_nome,
											tb.aluno_sexo,
											tb.aluno_data_nascimento,
											tb.prova_componente,
											tb.prova_caderno,
											tb.prova_quantidade_questoes,
											tb.aluno_frequencia,
											q.id AS questaoid,
											q.ordem + 1 AS questao_ordem,
											CASE
												WHEN qar.alternativa_id IS NOT NULL THEN a.numeracao
												ELSE qar.resposta
											END AS resposta,
											tb.prova_data_inicio,
											tb.prova_data_entregue
							FROM
							  ({queryDados}) as tb
							LEFT JOIN turma t ON t.id = tb.turma_id
							LEFT JOIN ue u ON u.id = t.ue_id
							LEFT JOIN dre d ON d.id = u.dre_id
							INNER JOIN questao q ON tb.prova_serap_estudantes_id = q.prova_id AND (NOT tb.possui_bib OR (tb.possui_bib AND tb.prova_caderno = q.caderno))
							LEFT JOIN f_questao_aluno_resposta_prova_aluno(tb.prova_serap_estudantes_id, q.id, tb.aluno_codigo_eol) qar ON 1 = 1
							LEFT JOIN alternativa a ON a.id = qar.alternativa_id
                            limit @take 
							offset @skip";
        }

        private string QueryAdesao(bool paraEstudanteComDeficiencia)
        {
            string joinDeficiencia = @"inner join aluno_deficiencia ad on ad.aluno_ra = a.ra
      						            inner join (select tpd.deficiencia_id  
										from prova p 
										inner join public.tipo_prova tp on p.tipo_prova_id = tp.id 
										inner join public.tipo_prova_deficiencia tpd on tp.id = tpd.tipo_prova_id 
										where p.prova_legado_id = @provaLegadoId) dp on dp.deficiencia_id = ad.deficiencia_id";

            return $@"SELECT tb_adesao_prova_turma_aluno.prova_serap_id,
							  tb_adesao_prova_turma_aluno.prova_serap_estudantes_id,
							  tb_adesao_prova_turma_aluno.aluno_codigo_eol,
							  tb_adesao_prova_turma_aluno.aluno_nome,
							  tb_adesao_prova_turma_aluno.aluno_sexo,
							  tb_adesao_prova_turma_aluno.aluno_data_nascimento,
							  tb_adesao_prova_turma_aluno.prova_componente,
							  tb_adesao_prova_turma_aluno.prova_caderno,
							  tb_adesao_prova_turma_aluno.prova_quantidade_questoes,
							  tb_adesao_prova_turma_aluno.aluno_frequencia,
							  tb_adesao_prova_turma_aluno.prova_data_inicio,
							  tb_adesao_prova_turma_aluno.prova_data_entregue,
							  tb_adesao_prova_turma_aluno.possui_bib,
							  CASE
								  WHEN tb_adesao_prova_turma_aluno.turma_id IS NULL THEN
										 (SELECT tah.turma_id
										  FROM turma_aluno_historico tah
										  WHERE tah.aluno_id = tb_adesao_prova_turma_aluno.aluno_serap_estudantes_id
											AND tah.ano_letivo = extract(YEAR FROM tb_adesao_prova_turma_aluno.inicio)
										  ORDER BY tah.data_matricula DESC
										  LIMIT 1)
								  ELSE tb_adesao_prova_turma_aluno.turma_id
							  END AS turma_id
					   FROM
						 (SELECT p.prova_legado_id AS prova_serap_id,
								 p.id AS prova_serap_estudantes_id,
								 a.ra AS aluno_codigo_eol,
								 a.id AS aluno_serap_estudantes_id,
								 CASE
									 WHEN a.nome_social IS NOT NULL THEN a.nome_social
									 ELSE a.nome
								 END AS aluno_nome,
								 a.sexo AS aluno_sexo,
								 a.data_nascimento AS aluno_data_nascimento,
								 CASE
									 WHEN p.disciplina IS NULL
										  OR p.multidisciplinar THEN 'Multidisciplinar'
									 ELSE p.disciplina
								 END AS prova_componente,
								 CASE
									 WHEN p.possui_bib THEN ca.caderno
									 ELSE ''
								 END AS prova_caderno,
								 p.total_itens AS prova_quantidade_questoes,
								 CASE
									 WHEN palu.frequencia = 0 THEN 'N'
									 WHEN palu.frequencia = 1 THEN 'P'
									 WHEN palu.frequencia = 2 THEN 'A'
									 WHEN palu.frequencia = 3 THEN 'R'
									 ELSE 'N'
								 END AS aluno_frequencia,
								 palu.criado_em AS prova_data_inicio,
								 palu.finalizado_em AS prova_data_entregue,
								 p.possui_bib,
								 p.inicio,
								 CASE
									 WHEN palu.id IS NULL THEN
											(SELECT tah.turma_id
											 FROM turma_aluno_historico tah
											 WHERE tah.aluno_id = a.id
											   AND tah.data_matricula::date <= p.fim::date
											   AND (tah.data_situacao::date >= p.inicio::date or tah.data_situacao is null)
											   AND tah.ano_letivo = extract(YEAR FROM p.inicio)
											 ORDER BY tah.data_matricula DESC
											 LIMIT 1)
									 ELSE
											(SELECT tah.turma_id
											 FROM turma_aluno_historico tah
											 WHERE tah.aluno_id = a.id
											   AND tah.data_matricula::date <= palu.finalizado_em::date
											   AND (tah.data_situacao::date >= p.inicio::date or tah.data_situacao is null)
											   AND tah.ano_letivo = extract(YEAR FROM p.inicio)
											 ORDER BY tah.data_matricula DESC
											 LIMIT 1)
								 END turma_id
						  FROM prova p
						  LEFT JOIN prova_adesao PAD ON pad.prova_id = p.id
						  LEFT JOIN aluno a ON a.ra = pad.aluno_ra
						  {(paraEstudanteComDeficiencia ? joinDeficiencia : string.Empty)}
						  LEFT JOIN caderno_aluno ca ON ca.prova_id = p.id AND ca.aluno_id = a.id
						  LEFT JOIN prova_aluno palu ON p.id = palu.prova_id AND a.ra = palu.aluno_ra AND palu.status in (2,5) AND palu.finalizado_em IS NOT NULL
						  WHERE p.aderir_todos = FALSE
							AND p.prova_legado_id = @provaLegadoId
							limit @take 
							offset @skip
					   ) as tb_adesao_prova_turma_aluno";
        }

        private string QueryAderirTodos(bool paraEstudanteComDeficiencia)
        {

            string joinDeficiencia = @"inner join aluno_deficiencia ad on ad.aluno_ra = vpta.aluno_ra
      								   inner join (select distinct tpd.deficiencia_id
												from prova p 
												inner join public.tipo_prova tp on p.tipo_prova_id = tp.id 
												inner join public.tipo_prova_deficiencia tpd on tp.id = tpd.tipo_prova_id 
												where p.prova_legado_id = @provaLegadoId) dp on dp.deficiencia_id = ad.deficiencia_id";

            return $@"			SELECT tb_prova_turma_aluno.prova_serap_id,
									  tb_prova_turma_aluno.prova_serap_estudantes_id,
									  a.ra AS aluno_codigo_eol,
									  CASE
										  WHEN a.nome_social IS NOT NULL THEN a.nome_social
										  ELSE a.nome
									  END AS aluno_nome,
									  a.sexo AS aluno_sexo,
									  a.data_nascimento AS aluno_data_nascimento,
									  tb_prova_turma_aluno.prova_componente,
									  CASE
										  WHEN tb_prova_turma_aluno.possui_bib THEN ca.caderno
										  ELSE ''
									  END AS prova_caderno,
									  tb_prova_turma_aluno.prova_quantidade_questoes,
									  CASE
										  WHEN palu.frequencia = 0 THEN 'N'
										  WHEN palu.frequencia = 1 THEN 'P'
										  WHEN palu.frequencia = 2 THEN 'A'
										  WHEN palu.frequencia = 3 THEN 'R'
										  ELSE 'N'
									  END AS aluno_frequencia,
									  palu.criado_em AS prova_data_inicio,
									  palu.finalizado_em AS prova_data_entregue,
									  tb_prova_turma_aluno.possui_bib,
									  CASE
										  WHEN palu.id IS NULL THEN
												 (SELECT tah.turma_id
												  FROM turma_aluno_historico tah
												  WHERE tah.aluno_id = a.id
													AND tah.data_matricula::date <= tb_prova_turma_aluno.fim::date
													AND (tah.data_situacao::date >= tb_prova_turma_aluno.inicio::date or tah.data_situacao is null)
													AND tah.ano_letivo = extract(YEAR FROM tb_prova_turma_aluno.inicio)
												  ORDER BY tah.data_matricula DESC
												  LIMIT 1)
										  ELSE
												 (SELECT tah.turma_id
												  FROM turma_aluno_historico tah
												  WHERE tah.aluno_id = a.id
													AND tah.data_matricula::date <= palu.finalizado_em::date
													AND (tah.data_situacao::date >= tb_prova_turma_aluno.inicio::date or tah.data_situacao is null)
													AND tah.ano_letivo = extract(YEAR FROM tb_prova_turma_aluno.inicio)
												  ORDER BY tah.data_matricula DESC
												  LIMIT 1)
									  END turma_id
							   FROM
								 (SELECT DISTINCT tb_prova_turma.prova_serap_estudantes_id,
												  tb_prova_turma.prova_serap_id,
												  tb_prova_turma.inicio,
												  tb_prova_turma.fim,
												  tb_prova_turma.prova_componente,
												  tb_prova_turma.prova_quantidade_questoes,
												  tb_prova_turma.possui_bib,
												  tah.aluno_id
								  FROM
									(SELECT DISTINCT vpta.prova_id AS prova_serap_estudantes_id,
													 vpta.prova_legado_id AS prova_serap_id,
													 vpta.turma_id,
													 p.inicio,
													 p.fim,
													 CASE
														 WHEN p.disciplina IS NULL
															  OR p.multidisciplinar THEN 'Multidisciplinar'
														 ELSE p.disciplina
													 END AS prova_componente,
													 p.total_itens AS prova_quantidade_questoes,
													 p.possui_bib
									 FROM v_prova_turma_aluno vpta
									 LEFT JOIN prova p ON p.id = vpta.prova_id
									 {(paraEstudanteComDeficiencia ? joinDeficiencia : string.Empty)}
									 WHERE vpta.aderir_todos = TRUE
									   AND vpta.prova_legado_id = @provaLegadoId
									limit @take 
									offset @skip
								  ) as tb_prova_turma
								  LEFT JOIN turma_aluno_historico tah ON tah.turma_id = tb_prova_turma.turma_id AND tah.data_matricula <= tb_prova_turma.fim AND (tah.data_situacao >= tb_prova_turma.inicio OR tah.data_situacao IS NULL) AND tah.ano_letivo = extract(YEAR FROM tb_prova_turma.inicio)
								 ) as tb_prova_turma_aluno
							   LEFT JOIN aluno a ON a.id = tb_prova_turma_aluno.aluno_id
							   LEFT JOIN caderno_aluno ca ON ca.prova_id = tb_prova_turma_aluno.prova_serap_estudantes_id AND ca.aluno_id = a.id
							   LEFT JOIN prova_aluno palu ON palu.prova_id = tb_prova_turma_aluno.prova_serap_estudantes_id AND palu.aluno_ra = a.ra AND palu.status in (2,5) AND palu.finalizado_em IS NOT null   
							limit @take 
							offset @skip";
        }

        public async Task CriarProvaRespostasExtracao(long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = $@"drop table if exists extracao_prova_{provaId};
                        select vape.*, q.id as questao_id, q.ordem + 1 as questa_ordem
                           into extracao_prova_{provaId} from v_aluno_prova_extracao vape
                           inner join questao q on vape.prova_serap_estudantes_id = q.prova_id and vape.prova_caderno = q.caderno 
                        where vape.prova_serap_id = @provaId;
                        
                        /*alter table extracao_prova_{provaId} add column questao_alternativa_id int8 null;
                        alter table extracao_prova_{provaId} add column questao_resposta text null;
                        alter table extracao_prova_{provaId} add column resposta text null;

                        update extracao_prova_{provaId} ep set questao_alternativa_id = qar.alternativa_id, questao_resposta = qar.resposta from questao_aluno_resposta qar where ep.questao_id = qar.questao_id and ep.aluno_codigo_eol = qar.aluno_ra;

                        update extracao_prova_{provaId} set resposta = questao_resposta where questao_resposta is not null;
                        update extracao_prova_{provaId} ep set resposta = a.numeracao from alternativa a where ep.questao_alternativa_id = a.id and ep.questao_alternativa_id is not null;
                        alter table extracao_prova_{provaId} drop column questao_alternativa_id;
                        alter table extracao_prova_{provaId} drop column questao_resposta;*/";

                await conn.ExecuteAsync(query, new { provaId });
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

        public async Task<Dominio.Prova> ObterPorIdLegadoAsync(long id)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select * from prova where prova_legado_id = @id";

                return await conn.QueryFirstOrDefaultAsync<Dominio.Prova>(query, new { id });
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<bool> VerificaSeExistePorProvaSerapId(long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select 1 from prova pa where prova_legado_id = @provaId";

                return await conn.QueryFirstOrDefaultAsync<bool>(query, new { provaId });
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<bool> VerificaSeExisteProvaFinalizadaPorId(long id)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select 1 from prova_aluno pa where prova_id = @id and finalizado_em is not null limit 1";

                return await conn.QueryFirstOrDefaultAsync<bool>(query, new { id });
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<bool> VerificaSeExisteRespostasPorId(long id)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select 1 
                                from prova p 
                                    inner join questao q on p.id = q.prova_id
                                    inner join questao_aluno_resposta qar on qar.questao_id = q.id
                                where p.id = @id
                                limit 1;";

                return await conn.QueryFirstOrDefaultAsync<bool>(query, new { id });
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<ProvaAlunoDto>> ObterProvasIniciadasPorModalidadeAsync(int modalidade)
        {
            using var conn = ObterConexao();
            try
            {
                int status = (int)ProvaStatus.Iniciado;
                var query = @"select
                                pa.id provaAlunoId,
                                pa.prova_id provaId,
                                pa.criado_em provaIniciadaEm,
                                pa.status,
                                p.modalidade,
                                p.inicio inicioProva,
                                p.fim fimProva,
                                p.tempo_execucao tempoExecucao
                            from 
                                prova p
                                inner join prova_aluno pa on p.id = pa.prova_id
                            where pa.status = @status
                            and p.modalidade = @modalidade
                            and (p.tempo_execucao > 0 or (p.tempo_execucao = 0 and NOW()::timestamp > p.fim))
                                order by p.id,pa.aluno_ra";

                return await conn.QueryAsync<ProvaAlunoDto>(query, new { modalidade, status });
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<bool> FinalizarProvaAsync(ProvaParaAtualizarDto provaParaAtualizar)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"update prova_aluno 
                                     set status = @status, 
                                     finalizado_em = @finalizadoEm,
                                     finalizado_em_servidor = @finalizadoEm
                                where prova_id = @provaId
                                and id = any(@ids)";

                await conn.ExecuteAsync(query,
                    new
                    {
                        provaParaAtualizar.ProvaId,
                        provaParaAtualizar.Status,
                        provaParaAtualizar.FinalizadoEm,
                        ids = provaParaAtualizar.IdsProvasAlunos
                    });

                return true;
            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<ProvaBIBSyncDto>> ObterProvasBibAsync()
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select p.id as ProvaId, p.total_cadernos as TotalCadernos
                              from prova p
                              where p.possui_bib = true";

                return await conn.QueryAsync<ProvaBIBSyncDto>(query);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<long?> ObterProvaOrigemCadernoAsync(long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select prova_id_origem_caderno from prova p where p.id = @provaId";

                return await conn.QueryFirstOrDefaultAsync<long?>(query, new { provaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }        
    }
}
