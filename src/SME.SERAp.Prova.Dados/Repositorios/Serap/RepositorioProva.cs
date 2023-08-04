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

        public async Task LimparDadosConsolidadosPorProvaSerapId(long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = $@"delete from resultado_prova_consolidado where prova_serap_id = @provaId;";
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

        public async Task LimparDadosConsolidadosPorFiltros(long provaId, string dreId, string ueId, string turmaCodigo)
        {
            using var conn = ObterConexao();
            try
            {
                var query = $@"call p_excluir_dados_consolidados_prova(@provaId, @dreId, @ueId, @turmaCodigo);";
                await conn.ExecuteAsync(query, new { provaId, dreId, ueId, turmaCodigo }, commandTimeout: 50000);
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
                var query = $@"call p_consolidar_dados_prova(@provaId, @dreId, @ueId, @turmaCodigo);";
                await conn.ExecuteAsync(query, new { provaId, dreId, ueId, turmaCodigo }, commandTimeout: 50000);
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
