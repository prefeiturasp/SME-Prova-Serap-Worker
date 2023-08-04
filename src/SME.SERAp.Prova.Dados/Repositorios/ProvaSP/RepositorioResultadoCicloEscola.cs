using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioResultadoCicloEscola : RepositorioProvaSpBase, IRepositorioResultadoCicloEscola
    {
        public RepositorioResultadoCicloEscola(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<ResultadoCicloEscola> ObterResultadoCicloEscola(string edicao, int areaConhecimentoId,
            string uadSigla, string escCodigo, int cicloId)
        {
            const string query = @"select rce.* 
                                    from [dbo].[ResultadoCicloEscola] rce with (NOLOCK)
                                    where rce.Edicao = @edicao
                                    and rce.AreaConhecimentoID = @areaConhecimentoId
                                    and rce.uad_sigla = @uadSigla
                                    and rce.esc_codigo = @escCodigo
                                    and rce.CicloID = @cicloId
                                    order by rce.Edicao desc";

            using var conn = ObterConexaoProvaSp();
            return await conn.QueryFirstOrDefaultAsync<ResultadoCicloEscola>(query,
                new { edicao, areaConhecimentoId, uadSigla, escCodigo = escCodigo.PadLeft(6, '0'), cicloId });
        }

        public async Task<long> IncluirAsync(ResultadoCicloEscola resultado)
        {
            const string query = @"insert into [dbo].[ResultadoCicloEscola]
                                    values(@edicao, @areaConhecimentoId, @uadSigla, @escCodigo, @cicloId, @valor,
                                        @nivelProficienciaId, @totalAlunos, @percentualAbaixoDoBasico,
                                        @percentualBasico, @percentualAdequado, @percentualAvancado,
                                        @percentualAlfabetizado)";

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

        public async Task<long> AlterarAsync(ResultadoCicloEscola resultado)
        {
            const string query = @"update [dbo].[ResultadoCicloEscola]
                                    set Valor = @valor,
                                        NivelProficienciaID = @nivelProficienciaId,
                                        TotalAlunos = @totalAlunos,
                                        PercentualAbaixoDoBasico = @percentualAbaixoDoBasico,
                                        PercentualBasico = @percentualBasico,
                                        PercentualAdequado = @percentualAdequado,
                                        PercentualAvancado = @percentualAvancado,
                                        PercentualAlfabetizado = @percentualAlfabetizado
                                    where Edicao = @edicao
                                    and AreaConhecimentoID = @areaConhecimentoId
                                    and uad_sigla = @uadSigla
                                    and esc_codigo = @escCodigo
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
        
        private static DynamicParameters ObterParametros(ResultadoCicloEscola resultado)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@edicao", resultado.Edicao, DbType.String, ParameterDirection.Input, 10);
            parametros.Add("@areaConhecimentoId", resultado.AreaConhecimentoId, DbType.Int16, ParameterDirection.Input);
            parametros.Add("@uadSigla", resultado.UadSigla, DbType.String, ParameterDirection.Input, 4);
            parametros.Add("@escCodigo", resultado.EscCodigo.PadLeft(6, '0'), DbType.String, ParameterDirection.Input, 20);
            parametros.Add("@cicloId", resultado.CicloId, DbType.Int32, ParameterDirection.Input, 4);
            parametros.Add("@valor", resultado.Valor, DbType.Decimal, ParameterDirection.Input, 5,6, 3);
            parametros.Add("@nivelProficienciaId", resultado.NivelProficienciaId, DbType.Int16, ParameterDirection.Input, 1);            
            parametros.Add("@totalAlunos", resultado.TotalAlunos, DbType.Int32, ParameterDirection.Input, 4);
            parametros.Add("@percentualAbaixoDoBasico", resultado.PercentualAbaixoDoBasico, DbType.Decimal, ParameterDirection.Input, 5,6, 2);
            parametros.Add("@percentualBasico", resultado.PercentualBasico, DbType.Decimal, ParameterDirection.Input, 5,6, 2);
            parametros.Add("@percentualAdequado", resultado.PercentualAdequado, DbType.Decimal, ParameterDirection.Input, 5,6, 2);
            parametros.Add("@percentualAvancado", resultado.PercentualAvancado, DbType.Decimal, ParameterDirection.Input, 5,6, 2);
            parametros.Add("@percentualAlfabetizado", resultado.PercentualAlfabetizado, DbType.Decimal, ParameterDirection.Input, 5,6, 2);
            return parametros;
        }        
    }
}