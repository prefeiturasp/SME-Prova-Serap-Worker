using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.Dtos;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioTurma : RepositorioBase<Turma>, IRepositorioTurma
    {
        public RepositorioTurma(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<IEnumerable<TurmaSgpDto>> ObterturmasSgpPorUeCodigo(string ueCodigo)
        {
            using var conn = ObterConexaoSgp();
            try
            {
                var query = @"select ano, 
                                     ano_letivo as anoLetivo,
                                     turma_id as codigo,
                                     tipo_turma as tipoturma,
                                     modalidade_codigo as modalidadeCodigo,
                                     turma.nome as nomeTurma,
                                     tipo_turno as tipoturno                                      
                                from turma
                               inner join ue on turma.ue_id = ue.id
                               where ue.ue_id = @ueCodigo
                                 and not historica
                                 and tipo_turma = 1
                                 and modalidade_codigo in (3,4,5,6)
                                 and ano_letivo = @anoLetivo ";

                var parametros = new
                {
                    ueCodigo,
                    anoLetivo = DateTime.Now.Year
                };

                return await conn.QueryAsync<TurmaSgpDto>(query, parametros);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<Turma> ObterturmaPorCodigo(string uecodigo)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select * 
                                from turma
                               where codigo = @uecodigo ";

                return await conn.QueryFirstOrDefaultAsync<Turma>(query, new { uecodigo });
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<Turma>> ObterTurmasPorAnoEAnoLetivo(int ano, int anoLetivo)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select * 
                                from turma
                               where ano = @ano and ano_letivo = @anoLetivo ";

                return await conn.QueryAsync<Turma>(query, new { ano = ano.ToString(), anoLetivo });
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<long> InserirOuAtualizarTurmaAsync(TurmaSgpDto turmaSgp)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"WITH upsert AS (
                            UPDATE turma
                            SET
                                ano = @ano, 
		                          ano_letivo = @anoLetivo, 
		                          ue_id = @ueId, 
		                          tipo_turma = @tipoTurma, 
		                          modalidade_codigo = @modalidade,
		                          nome = @nome,
		                          tipo_turno = @tipoTurno,
		                          data_atualizacao = @dataAtualizacao
                            WHERE
                                codigo = @codigo
                            RETURNING *
                        )
                        INSERT INTO turma(ano, ano_letivo, codigo, ue_id, tipo_turma, modalidade_codigo, nome, tipo_turno, data_atualizacao)
                        SELECT
                            @ano,  @anoLetivo, @codigo, @ueId, @tipoTurma,   @modalidade,  @nome, @tipoTurno, @dataAtualizacao
                        WHERE
                            NOT EXISTS (SELECT 1 FROM upsert);
                        select id from turma where codigo = @codigo;";

                return await conn.QueryFirstOrDefaultAsync<long>(query, new { ano = turmaSgp.Ano, 
                    anoLetivo = turmaSgp.AnoLetivo, 
                    codigo = turmaSgp.Codigo,
                    ueId = turmaSgp.UeId,
                    tipoTurma = turmaSgp.TipoTurma,
                    modalidade = turmaSgp.ModalidadeCodigo,
                    nome = turmaSgp.NomeTurma,
                    tipoTurno = turmaSgp.TipoTurno,
                    dataAtualizacao = DateTime.Now
                });
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
