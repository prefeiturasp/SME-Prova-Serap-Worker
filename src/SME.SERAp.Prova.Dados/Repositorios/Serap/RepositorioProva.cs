using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioProva : RepositorioBase<Dominio.Prova>, IRepositorioProva
    {
        public RepositorioProva(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }
        
        public async Task LimparDadosConsolidadosPorFiltros(long provaId, string dreId, string ueId, string turmaCodigo)
        {
            using var conn = ObterConexao();
            try
            {
	            const string query = @"call p_excluir_dados_consolidados_prova(@provaId, @dreId, @ueId, @turmaCodigo);";
	            await conn.ExecuteAsync(query, new { provaId, dreId, ueId, turmaCodigo }, commandTimeout: 50000);
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

        public async Task ConsolidarProvaRespostasPorFiltros(long provaId, string dreId, string ueId, string turmaCodigo)
        {
            using var conn = ObterConexao();
            try
            {
	            const string query = "call p_consolidar_dados_prova(@provaId, @dreId, @ueId, @turmaCodigo);";
	            await conn.ExecuteAsync(query, new { provaId, dreId, ueId, turmaCodigo }, commandTimeout: 50000);
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



        public async Task ConsolidarProvaRespostasAdesaoManual(long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = $@"call p_consolidar_dados_prova_adesao_manual(@provaId);";
                await conn.ExecuteAsync(query, new { provaId }, commandTimeout: 50000);
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

        public async Task ConsolidarProvaRespostasPorProvaSerapId(long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = $@"
                            insert into resultado_prova_consolidado  
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
                        where vape.prova_serap_id = @provaId;";

                await conn.ExecuteAsync(query, new { provaId }, commandTimeout: 50000);

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
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<ProvaTaiSyncDto>> ObterProvasTaiAsync()
        {
            using var conn = ObterConexao();
            try
            {
                const string query = @"select p.id as ProvaId,
                                        p.prova_legado_id as ProvaLegadoId,
                                        p.disciplina,
                                        pao.ano
                                        from prova p
                                        inner join prova_ano_original pao on pao.prova_id = p.id
                                        where p.formato_tai = true";

                return await conn.QueryAsync<ProvaTaiSyncDto>(query);
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

        public async Task<IEnumerable<ProvaAtualizadaDto>> ObterProvaPorUltimaAtualizacao(DateTime dataBase)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select p.id as provaId, p.ultima_atualizacao as UltimaAtualizacao
                              from prova p
                              where p.ultima_atualizacao > @dataBase";

                return await conn.QueryAsync<ProvaAtualizadaDto>(query, new { dataBase });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<bool> VerificaSePossuiDownload(long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select 1
                              from prova p
                              inner join downloads_prova_aluno dpa on dpa.prova_id = p.id
                              where p.id = @provaId limit 1"
                ;

                return await conn.QueryFirstOrDefaultAsync<bool>(query, new { provaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<Dominio.Prova>> ObterProvasLiberadasNoPeriodoParaCacheAsync()
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                const string query = "select * from prova where inicio_download::date <= current_date and fim::date >= current_date";
                return await conn.QueryAsync<Dominio.Prova>(query);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<bool> VerificaSePossuiTipoDeficiencia(long provaLegadoId)
        {
            using var conn = ObterConexao();
            try
            {
	            const string query = @"select 1
										from prova p
                                		inner join tipo_prova tp on p.tipo_prova_id = tp.id
                                    		and tp.para_estudante_com_deficiencia
                                   		where p.prova_legado_id = @ProvaLegadoId";

	            return await conn.QueryFirstOrDefaultAsync<bool>(query, new { provaLegadoId });
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

        public async Task<IEnumerable<ConsolidadoAlunoProvaDto>> ObterAlunosProvaAdesaoTodosPorProvaLegadoIdETurmasCodigos(long provaLegadoId, string[] turmasCodigos)
        {
            using var conn = ObterConexao();
            try
            {
	            const string query = @"with tb_prova_turma as (
											select distinct vpta.prova_legado_id as prova_serap_id,
												vpta.prova_id as prova_serap_estudantes_id,												
												vpta.turma_id,
												vpta.aluno_id,
												p.inicio,
												p.fim,
												case
													when p.disciplina is null
													or p.multidisciplinar then 'Multidisciplinar'
													else p.disciplina
												end as prova_componente,
												p.total_itens as prova_quantidade_questoes,
												p.possui_bib
											from v_prova_turma_aluno vpta
											left join prova p on p.id = vpta.prova_id
											where vpta.aderir_todos = true
											and vpta.prova_legado_id = @provaLegadoId
										),                                    
										tb_prova_turma_aluno as (
											select distinct tb_prova_turma.prova_serap_estudantes_id,
												tb_prova_turma.prova_serap_id,
												tb_prova_turma.inicio,
												tb_prova_turma.fim,
												tb_prova_turma.prova_componente,
												tb_prova_turma.prova_quantidade_questoes,
												tb_prova_turma.possui_bib,
												coalesce(tah.aluno_id, tb_prova_turma.aluno_id) as aluno_id
											from tb_prova_turma
											left join turma_aluno_historico tah on tah.turma_id = tb_prova_turma.turma_id
												and tah.data_matricula <= tb_prova_turma.fim
												and (tah.data_situacao >= tb_prova_turma.inicio or tah.data_situacao is null)
												and tah.ano_letivo = extract(year from tb_prova_turma.inicio)	
											left join turma t on t.id = coalesce(tah.turma_id, tb_prova_turma.turma_id)
										),
										tb_prova_turma_aluno_adesao_todos as (
											select tb_prova_turma_aluno.prova_serap_id as ProvaSerapId,
												tb_prova_turma_aluno.prova_serap_estudantes_id as ProvaSerapEstudantesId,
												a.ra as AlunoCodigoEol,
												coalesce(a.nome_social, a.nome) as AlunoNome,
												a.sexo as AlunoSexo,
												a.data_nascimento as AlunoDataNascimento,
												tb_prova_turma_aluno.prova_componente as ProvaComponente,
												case
													when tb_prova_turma_aluno.possui_bib then ca.caderno
													else ''
												end as ProvaCaderno,
												tb_prova_turma_aluno.prova_quantidade_questoes as ProvaQuantidadeQuestoes,
												case
													when palu.frequencia = 0 then 'N'
													when palu.frequencia = 1 then 'P'
													when palu.frequencia = 2 then 'A'
													when palu.frequencia = 3 then 'R'
													else 'N'
												end as AlunoFrequencia,
												palu.criado_em as ProvaDataInicio,
												palu.finalizado_em as ProvaDataEntregue,
												tb_prova_turma_aluno.possui_bib as PossuiBib,
												coalesce(case
															when palu.id is null then
																(select tah.turma_id
																	from turma_aluno_historico tah
																	inner join turma t on t.id = tah.turma_id 
																	where tah.aluno_id = a.id
																	and tah.data_matricula::date <= tb_prova_turma_aluno.fim::date
																	and (tah.data_situacao::date >= tb_prova_turma_aluno.inicio::date or tah.data_situacao is null)
																	and tah.ano_letivo = extract(year from tb_prova_turma_aluno.inicio)
																	order by tah.data_matricula desc
																	limit 1)
															else (select tah.turma_id
																	from turma_aluno_historico tah
																	inner join turma t on t.id = tah.turma_id
																	where tah.aluno_id = a.id
																	and tah.data_matricula::date <= palu.finalizado_em::date
																	and (tah.data_situacao::date >= tb_prova_turma_aluno.inicio::date or tah.data_situacao is null)
																	and tah.ano_letivo = extract(year from tb_prova_turma_aluno.inicio)																	
																	order by tah.data_matricula desc
																	limit 1)
														end, a.turma_id) as TurmaId,
												a.situacao as AlunoSituacao
											from tb_prova_turma_aluno
											left join aluno a on a.id = tb_prova_turma_aluno.aluno_id
											left join caderno_aluno ca on ca.prova_id = tb_prova_turma_aluno.prova_serap_estudantes_id and ca.aluno_id = a.id
											left join prova_aluno palu on palu.prova_id = tb_prova_turma_aluno.prova_serap_estudantes_id and palu.aluno_ra = a.ra
												and palu.status in (2, 5, 6, 7)
												and palu.finalizado_em is not null
											left join aluno_deficiencia ad on ad.aluno_ra = a.ra
											left join tipo_deficiencia td on td.id = ad.deficiencia_id
											where (td.id is null or td.prova_normal)
										)	
			                            select distinct p.*,
											dre.dre_id as DreCodigoEol,
											dre.abreviacao as DreSigla,
											dre.nome as DreNome,
											ue.ue_id as UeCodigoEol,
											(CASE
												WHEN ue.tipo_escola = 1 THEN 'EMEF'::text
					                            WHEN ue.tipo_escola = 2 THEN 'EMEI'::text
					                            WHEN ue.tipo_escola = 3 THEN 'EMEFM'::text
					                            WHEN ue.tipo_escola = 4 THEN 'EMEBS'::text
					                            WHEN ue.tipo_escola = 10 THEN 'CEI DIRET'::text
					                            WHEN ue.tipo_escola = 11 THEN 'CEI INDIR'::text
					                            WHEN ue.tipo_escola = 12 THEN 'CR.P.CONV'::text
					                            WHEN ue.tipo_escola = 13 THEN 'CIEJA'::text
					                            WHEN ue.tipo_escola = 14 THEN 'CCI/CIPS'::text
					                            WHEN ue.tipo_escola = 15 THEN 'ESC.PART.'::text
					                            WHEN ue.tipo_escola = 16 THEN 'CEU EMEF'::text
					                            WHEN ue.tipo_escola = 17 THEN 'CEU EMEI'::text
					                            WHEN ue.tipo_escola = 18 THEN 'CEU CEI'::text
					                            WHEN ue.tipo_escola = 19 THEN 'CEU'::text
					                            WHEN ue.tipo_escola = 22 THEN 'MOVA'::text
					                            WHEN ue.tipo_escola = 23 THEN 'CMCT'::text
					                            WHEN ue.tipo_escola = 25 THEN 'E TEC'::text
					                            WHEN ue.tipo_escola = 26 THEN 'ESP CONV'::text
					                            WHEN ue.tipo_escola = 27 THEN 'CEU AT COMPL'::text
					                            WHEN ue.tipo_escola = 29 THEN 'CCA'::text
					                            WHEN ue.tipo_escola = 28 THEN 'CEMEI'::text
					                            WHEN ue.tipo_escola = 30 THEN 'CECI'::text
					                            WHEN ue.tipo_escola = 31 THEN 'CEU CEMEI'::text
					                            WHEN ue.tipo_escola = 32 THEN 'EMEF'::text
					                            WHEN ue.tipo_escola = 33 THEN 'EMEI'::text
												ELSE NULL::text
											END || ' '::text) || ue.nome::text as UeNome,
											t.ano as TurmaAnoEscolar,
											t.codigo AS TurmaCodigo,
											t.nome AS TurmaDescricao                            
										from tb_prova_turma_aluno_adesao_todos p
											inner join turma t on t.id = p.TurmaId
											inner join ue on ue.id = t.ue_id  
											inner join dre on dre.id = ue.dre_id
										where t.codigo in @turmasCodigos";

	            return await conn.QueryAsync<ConsolidadoAlunoProvaDto>(query, new { provaLegadoId, turmasCodigos });
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

        public async Task<IEnumerable<ConsolidadoAlunoProvaDto>> ObterAlunosProvaAdesaoManualPorProvaLegadoIdETurmasCodigos(long provaLegadoId, string[] turmasCodigos)
        {
            using var conn = ObterConexao();
            try
            {
	            const string query = @"with tb_prova_turma_aluno_adesao_manual as (
											SELECT p.prova_legado_id AS ProvaSerapId,
												p.id AS ProvaSerapEstudantesId,
                        						a.ra AS AlunoCodigoEol,
                       							COALESCE(a.nome_social, a.nome) AS AlunoNome,
                       							a.sexo AS AlunoSexo,
	                       						a.data_nascimento AS AlunoDataNascimento,
		                   						CASE
			               	    					WHEN p.disciplina IS NULL OR p.multidisciplinar THEN 'Multidisciplinar'
				           	    					ELSE p.disciplina
					       						END AS ProvaComponente,
						   						CASE
                									WHEN p.possui_bib THEN ca.caderno
                	    							ELSE ''
                								END AS ProvaCaderno,
                								p.total_itens AS ProvaQuantidadeQuestoes,
                								CASE
                	    							WHEN palu.frequencia = 0 THEN 'N'
                	    							WHEN palu.frequencia = 1 THEN 'P'
                	    							WHEN palu.frequencia = 2 THEN 'A'
                	    							WHEN palu.frequencia = 3 THEN 'R'
                	    							ELSE 'N'
                	        					END AS AlunoFrequencia,
                	        					palu.criado_em AS ProvaDataInicio,
                	        					palu.finalizado_em AS ProvaDataEntregue,
                	        					p.possui_bib as PossuiBib,
                	        					COALESCE(CASE
                	            							WHEN palu.id IS NULL THEN
                	                							(SELECT tah.turma_id
                	                    							FROM turma_aluno_historico tah
                	                    							WHERE tah.aluno_id = a.id
                	                    							AND tah.data_matricula::date <= p.fim::date
                	                    							AND (tah.data_situacao::date >= p.inicio::date or tah.data_situacao is null)
                	                    							AND tah.ano_letivo = extract(YEAR FROM p.inicio)
                	                    							ORDER BY tah.data_matricula DESC
                	                    							LIMIT 1)
                	            							ELSE (SELECT tah.turma_id
	                	                    						FROM turma_aluno_historico tah
		            	                    						WHERE tah.aluno_id = a.id
			        	                    						AND tah.data_matricula::date <= palu.finalizado_em::date
				    	                    						AND (tah.data_situacao::date >= p.inicio::date or tah.data_situacao is null)
						                    						AND tah.ano_letivo = extract(YEAR FROM p.inicio)
                		                    						ORDER BY tah.data_matricula DESC
                			                						LIMIT 1)
                										END, a.turma_id) as TurmaId,
                	        					a.situacao as AlunoSituacao
                							FROM prova p
                							LEFT JOIN prova_adesao PAD ON pad.prova_id = p.id
	                						LEFT JOIN aluno a ON a.ra = pad.aluno_ra
		            						LEFT JOIN caderno_aluno ca ON ca.prova_id = p.id
			        	    					AND ca.aluno_id = a.id
				    						LEFT JOIN prova_aluno palu ON p.id = palu.prova_id
						    					AND a.ra = palu.aluno_ra
                		    					AND palu.status in (2, 5, 6, 7)
                								AND palu.finalizado_em IS NOT null
                							WHERE p.aderir_todos = false
                							AND p.prova_legado_id = @provaLegadoId)
											select tb.*,
												dre.dre_id AS DreCodigoEol,
												dre.abreviacao AS DreSigla,
												dre.nome AS DreNome,
												ue.ue_id AS UeCodigoEol,
                    							(CASE
                            						WHEN ue.tipo_escola = 1 THEN 'EMEF'::text
                            						WHEN ue.tipo_escola = 2 THEN 'EMEI'::text
						                            WHEN ue.tipo_escola = 3 THEN 'EMEFM'::text
						                            WHEN ue.tipo_escola = 4 THEN 'EMEBS'::text
						                            WHEN ue.tipo_escola = 10 THEN 'CEI DIRET'::text
						                            WHEN ue.tipo_escola = 11 THEN 'CEI INDIR'::text
						                            WHEN ue.tipo_escola = 12 THEN 'CR.P.CONV'::text
						                            WHEN ue.tipo_escola = 13 THEN 'CIEJA'::text
						                            WHEN ue.tipo_escola = 14 THEN 'CCI/CIPS'::text
						                            WHEN ue.tipo_escola = 15 THEN 'ESC.PART.'::text
						                            WHEN ue.tipo_escola = 16 THEN 'CEU EMEF'::text
						                            WHEN ue.tipo_escola = 17 THEN 'CEU EMEI'::text
						                            WHEN ue.tipo_escola = 18 THEN 'CEU CEI'::text
						                            WHEN ue.tipo_escola = 19 THEN 'CEU'::text
						                            WHEN ue.tipo_escola = 22 THEN 'MOVA'::text
						                            WHEN ue.tipo_escola = 23 THEN 'CMCT'::text
						                            WHEN ue.tipo_escola = 25 THEN 'E TEC'::text
						                            WHEN ue.tipo_escola = 26 THEN 'ESP CONV'::text
						                            WHEN ue.tipo_escola = 27 THEN 'CEU AT COMPL'::text
						                            WHEN ue.tipo_escola = 29 THEN 'CCA'::text
						                            WHEN ue.tipo_escola = 28 THEN 'CEMEI'::text
						                            WHEN ue.tipo_escola = 30 THEN 'CECI'::text
						                            WHEN ue.tipo_escola = 31 THEN 'CEU CEMEI'::text
						                            WHEN ue.tipo_escola = 32 THEN 'EMEF'::text
						                            WHEN ue.tipo_escola = 33 THEN 'EMEI'::text
                            						ELSE NULL::text
                        						END || ' '::text) || ue.nome::text AS UeNome,
						  						t.ano as TurmaAnoEscolar,
							   					CASE
                            						WHEN t.ano::text <> 'S'::text THEN (t.ano::text || 'ano'::text)::CHARACTER varying
                            						ELSE t.ano
                            					END AS TurmaAnoEscolar,
                            					t.codigo AS TurmaCodigo,
                            					t.nome AS TurmaDescricao												
											from tb_prova_turma_aluno_adesao_manual tb
											JOIN turma t ON t.id = tb.TurmaId
											JOIN ue ON t.ue_id = ue.id
											JOIN dre ON ue.dre_id = dre.id
											WHERE t.codigo in @turmasCodigos";

	            return await conn.QueryAsync<ConsolidadoAlunoProvaDto>(query, new { provaLegadoId, turmasCodigos });
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

        public async Task<IEnumerable<ConsolidadoAlunoProvaDto>> ObterAlunosProvaDeficienciaPorProvaLegadoIdETurmasCodigos(long provaLegadoId, string[] turmasCodigos)
        {
            using var conn = ObterConexao();

            try
            {
                const string query = @"with tb_prova_turma as (
                							select distinct vpta.prova_id as prova_serap_estudantes_id,
                								vpta.prova_legado_id as prova_serap_id,
                								vpta.turma_id,
                								vpta.aluno_id,
                								vpta.aluno_ra,
                								p.inicio,
                								p.fim,
                								case
                									when p.disciplina is null
                									or p.multidisciplinar then 'Multidisciplinar'
                									else p.disciplina
                								end as prova_componente,
                								p.total_itens as prova_quantidade_questoes,
                								p.possui_bib
                							from v_prova_turma_aluno vpta
                							left join prova p on p.id = vpta.prova_id
                							where vpta.aderir_todos = true
                							and vpta.prova_legado_id = @provaLegadoId											
										),                     
						                tb_deficiencias as (
                							select td.id as deficiencia_id, p.prova_legado_id 
                							from prova p
                							inner join tipo_prova tp on tp.id = p.tipo_prova_id
                							inner join tipo_prova_deficiencia tpd on tpd.tipo_prova_id = tp.id  
                							inner join tipo_deficiencia td on td.id = tpd.deficiencia_id 
                							where p.prova_legado_id =  @provaLegadoId
						                ),
										tb_prova_turma_aluno as (
                							select distinct tb_prova_turma.prova_serap_estudantes_id,
                								tb_prova_turma.prova_serap_id,
                								tb_prova_turma.inicio,
                								tb_prova_turma.fim,
                								tb_prova_turma.prova_componente,
                								tb_prova_turma.prova_quantidade_questoes,
                								tb_prova_turma.possui_bib,
                								coalesce(tah.aluno_id, tb_prova_turma.aluno_id) as aluno_id
                							from tb_prova_turma
                							left join aluno a on a.id = tb_prova_turma.aluno_id		
                							left join turma_aluno_historico tah on tah.turma_id = tb_prova_turma.turma_id
                								and tah.data_matricula <= tb_prova_turma.fim
                								and (tah.data_situacao >= tb_prova_turma.inicio or tah.data_situacao is null or tah.data_situacao <= a.data_atualizacao)
                								and tah.ano_letivo = extract(year from tb_prova_turma.inicio)	
                							left join turma t on t.id = coalesce(tah.turma_id, tb_prova_turma.turma_id)
                							where (exists (select 1
                										from aluno_deficiencia ad
                										where ad.aluno_ra = tb_prova_turma.aluno_ra
                										and ad.deficiencia_id in (select deficiencia_id from tb_deficiencias))
                									or exists (select 1 
                												from aluno_deficiencia ad 
                												join aluno a on a.ra = ad.aluno_ra
                												where a.id = coalesce(tah.aluno_id, tb_prova_turma.aluno_id) 
                												and ad.deficiencia_id in (select deficiencia_id from tb_deficiencias)))
										),
										tb_prova_turma_aluno_adesao_todos as (
                							select tb_prova_turma_aluno.prova_serap_id as ProvaSerapId,
                								tb_prova_turma_aluno.prova_serap_estudantes_id as ProvaSerapEstudantesId,
                								a.ra as AlunoCodigoEol,
                								coalesce(a.nome_social, a.nome) as AlunoNome,
                								a.sexo as AlunoSexo,
                								a.data_nascimento as AlunoDataNascimento,
                								tb_prova_turma_aluno.prova_componente as ProvaComponente,
                								case
                									when tb_prova_turma_aluno.possui_bib then ca.caderno
                									else ''
                								end as ProvaCaderno,
                								tb_prova_turma_aluno.prova_quantidade_questoes as ProvaQuantidadeQuestoes,
                								case
                									when palu.frequencia = 0 then 'N'
                									when palu.frequencia = 1 then 'P'
                									when palu.frequencia = 2 then 'A'
                									when palu.frequencia = 3 then 'R'
                									else 'N'
                								end as AlunoFrequencia,
                								palu.criado_em as ProvaDataInicio,
                								palu.finalizado_em as ProvaDataEntregue,
                								tb_prova_turma_aluno.possui_bib as PossuiBib,
                								coalesce(case
                									when palu.id is null then
                										(select tah.turma_id
                											from turma_aluno_historico tah
                											inner join turma t on t.id = tah.turma_id 
                											where tah.aluno_id = a.id
                											and tah.data_matricula::date <= tb_prova_turma_aluno.fim::date
                											and (tah.data_situacao::date >= tb_prova_turma_aluno.inicio::date or tah.data_situacao is null)
                											and tah.ano_letivo = extract(year from tb_prova_turma_aluno.inicio)
                											order by tah.data_matricula desc
															limit 1)
                									else
                										(select tah.turma_id
                											from turma_aluno_historico tah
                											inner join turma t on t.id = tah.turma_id
                											where tah.aluno_id = a.id
                											and tah.data_matricula::date <= palu.finalizado_em::date
                											and (tah.data_situacao::date >= tb_prova_turma_aluno.inicio::date or tah.data_situacao is null)
                											and tah.ano_letivo = extract(year from tb_prova_turma_aluno.inicio)
                											order by tah.data_matricula desc
															limit 1)
                								end, a.turma_id) as TurmaId,
                								a.situacao as AlunoSituacao
                							from tb_prova_turma_aluno
                							left join aluno a on a.id = tb_prova_turma_aluno.aluno_id
                							left join caderno_aluno ca on ca.prova_id = tb_prova_turma_aluno.prova_serap_estudantes_id
												and ca.aluno_id = a.id
                							left join prova_aluno palu on palu.prova_id = tb_prova_turma_aluno.prova_serap_estudantes_id 
												and palu.aluno_ra = a.ra
                								and palu.status in (2, 5, 6, 7)
                								and palu.finalizado_em is not null
										)		
			                            select distinct p.*,
											dre.dre_id as DreCodigoEol,
											dre.abreviacao as DreSigla,
											dre.nome as DreNome,
											ue.ue_id as UeCodigoEol,
											(CASE
												WHEN ue.tipo_escola = 1 THEN 'EMEF'::text
					                            WHEN ue.tipo_escola = 2 THEN 'EMEI'::text
					                            WHEN ue.tipo_escola = 3 THEN 'EMEFM'::text
					                            WHEN ue.tipo_escola = 4 THEN 'EMEBS'::text
					                            WHEN ue.tipo_escola = 10 THEN 'CEI DIRET'::text
					                            WHEN ue.tipo_escola = 11 THEN 'CEI INDIR'::text
					                            WHEN ue.tipo_escola = 12 THEN 'CR.P.CONV'::text
					                            WHEN ue.tipo_escola = 13 THEN 'CIEJA'::text
					                            WHEN ue.tipo_escola = 14 THEN 'CCI/CIPS'::text
					                            WHEN ue.tipo_escola = 15 THEN 'ESC.PART.'::text
					                            WHEN ue.tipo_escola = 16 THEN 'CEU EMEF'::text
					                            WHEN ue.tipo_escola = 17 THEN 'CEU EMEI'::text
					                            WHEN ue.tipo_escola = 18 THEN 'CEU CEI'::text
					                            WHEN ue.tipo_escola = 19 THEN 'CEU'::text
					                            WHEN ue.tipo_escola = 22 THEN 'MOVA'::text
					                            WHEN ue.tipo_escola = 23 THEN 'CMCT'::text
					                            WHEN ue.tipo_escola = 25 THEN 'E TEC'::text
					                            WHEN ue.tipo_escola = 26 THEN 'ESP CONV'::text
					                            WHEN ue.tipo_escola = 27 THEN 'CEU AT COMPL'::text
					                            WHEN ue.tipo_escola = 29 THEN 'CCA'::text
					                            WHEN ue.tipo_escola = 28 THEN 'CEMEI'::text
					                            WHEN ue.tipo_escola = 30 THEN 'CECI'::text
					                            WHEN ue.tipo_escola = 31 THEN 'CEU CEMEI'::text
					                            WHEN ue.tipo_escola = 32 THEN 'EMEF'::text
					                            WHEN ue.tipo_escola = 33 THEN 'EMEI'::text
												ELSE NULL::text
											END || ' '::text) || ue.nome::text as UeNome,
											t.ano as TurmaAnoEscolar,
											t.codigo AS TurmaCodigo,
											t.nome AS TurmaDescricao                            
										from tb_prova_turma_aluno_adesao_todos p
											inner join turma t on t.id = p.TurmaId 
											inner join ue on ue.id = t.ue_id  
											inner join dre on dre.id = ue.dre_id
										where t.codigo in @turmasCodigos";

                return await conn.QueryAsync<ConsolidadoAlunoProvaDto>(query, new { provaLegadoId, turmasCodigos });
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
