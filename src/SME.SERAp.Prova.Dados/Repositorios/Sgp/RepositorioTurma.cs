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
                                 and modalidade_codigo = @modalidadeCodigo
                                 and ano_letivo = @anoLetivo ";

                var parametros = new
                {
                    ueCodigo,
                    modalidadeCodigo = (int)Modalidade.Fundamental,
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
    }
}
