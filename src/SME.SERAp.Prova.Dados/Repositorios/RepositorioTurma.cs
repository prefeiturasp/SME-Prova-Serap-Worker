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

        public async Task<IEnumerable<TurmaSgpDto>> ObterturmasSgpPorUeId(long ueId)
        {
            using var conn = ObterConexaoSgp();
            try
            {
                var query = @"select ano, 
                                     ano_letivo as anoLetivo,
                                     turma_id as codigo,
                                     tipo_turma as tipoturma,
                                     modalidade_codigo as modalidadeCodigo,
                                     nome as nomeTurma,
                                     tipo_turno as tipoturno                                      
                                from turma
                               where ue_id = @ueId
                                 and not historica 
                                 and modalidade_codigo = @modalidadeCodigo
                                 and ano_letivo = @anoLetivo ";

                var parametros = new
                {
                    ueId,
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
    }
}
