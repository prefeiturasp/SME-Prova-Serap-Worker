using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioResultadoCicloSme : RepositorioProvaSpBase, IRepositorioResultadoCicloSme
    {
        public RepositorioResultadoCicloSme(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }        
        
        public async Task<ResultadoCicloSme> ObterResultadoCicloSme(string edicao, int areaConhecimentoId, int cicloId)
        {
            const string query = @"select rcs.* 
                                    from [dbo].[ResultadoCicloSme] rcs with (NOLOCK)
                                    where rcs.Edicao = @edicao
                                    and rcs.AreaConhecimentoID = @areaConhecimentoId
                                    and rcs.CicloID = @cicloId
                                    order by rcs.Edicao desc";

            using var conn = ObterConexaoProvaSp();
            return await conn.QueryFirstOrDefaultAsync<ResultadoCicloSme>(query,
                new { edicao, areaConhecimentoId, cicloId });
        }

        public async Task<long> IncluirAsync(ResultadoCicloSme resultado)
        {
            const string query = @"insert into [dbo].[ResultadoCicloSme]
                                    values(@edicao, @areaConhecimentoId, @cicloId, @valor, @totalAlunos, 
                                        @nivelProficienciaId, @percentualAbaixoDoBasico, @percentualBasico,
                                        @percentualAdequado, @percentualAvancado, @percentualAlfabetizado)";

            using var conn = ObterConexaoProvaSp();
            try
            {
                var parametros = ObterParametros(resultado);
                return await conn.ExecuteAsync(query, parametros);
            }
            catch (Exception e)
            {
                var log = $@"Incluir {resultado.GetType().Name} -- Erro:{e.Message} ---- Objeto: {ResultadoPsp.ObterJsonObjetoResultado(resultado)}";
                throw new Exception(log);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<long> AlterarAsync(ResultadoCicloSme resultado)
        {
            const string query = @"update [dbo].[ResultadoCicloSme]
                                    set Valor = @valor,
                                        TotalAlunos = @totalAlunos,
                                        NivelProficienciaID = @nivelProficienciaId,
                                        PercentualAbaixoDoBasico = @percentualAbaixoDoBasico,
                                        PercentualBasico = @percentualBasico,
                                        PercentualAdequado = @percentualAdequado,
                                        PercentualAvancado = @percentualAvancado,
                                        PercentualAlfabetizado = @percentualAlfabetizado
                                    where Edicao = @edicao
                                    and AreaConhecimentoID = @areaConhecimentoId
                                    and CicloID = @cicloId";

            var conn = ObterConexaoProvaSp();
            try
            {
                var parametros = ObterParametros(resultado);
                return await conn.ExecuteAsync(query, parametros);
            }
            catch (Exception e)
            {
                var log = $@"Alterar {resultado.GetType().Name} -- Erro:{e.Message} ---- Objeto: {ResultadoPsp.ObterJsonObjetoResultado(resultado)}";
                throw new Exception(log);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private static DynamicParameters ObterParametros(ResultadoCicloSme resultado)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@edicao", resultado.Edicao, DbType.String, ParameterDirection.Input, 10);
            parametros.Add("@areaConhecimentoId", resultado.AreaConhecimentoId, DbType.Int16, ParameterDirection.Input);
            parametros.Add("@cicloId", resultado.CicloId, DbType.Int32, ParameterDirection.Input, 4);
            parametros.Add("@valor", resultado.Valor, DbType.Decimal, ParameterDirection.Input, 5,6, 3);
            parametros.Add("@totalAlunos", resultado.TotalAlunos, DbType.Int32, ParameterDirection.Input, 4);
            parametros.Add("@nivelProficienciaId", resultado.NivelProficienciaId, DbType.Int16, ParameterDirection.Input);
            parametros.Add("@percentualAbaixoDoBasico", resultado.PercentualAbaixoDoBasico, DbType.Decimal, ParameterDirection.Input, 5,6, 2);
            parametros.Add("@percentualBasico", resultado.PercentualBasico, DbType.Decimal, ParameterDirection.Input, 5,6, 2);
            parametros.Add("@percentualAdequado", resultado.PercentualAdequado, DbType.Decimal, ParameterDirection.Input, 5,6, 2);
            parametros.Add("@percentualAvancado", resultado.PercentualAvancado, DbType.Decimal, ParameterDirection.Input, 5,6, 2);
            parametros.Add("@percentualAlfabetizado", resultado.PercentualAlfabetizado, DbType.Decimal, ParameterDirection.Input, 5,6, 2);
            return parametros;
        }
    }
}