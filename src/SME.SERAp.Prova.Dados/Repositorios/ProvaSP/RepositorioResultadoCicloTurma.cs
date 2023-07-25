using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioResultadoCicloTurma : RepositorioProvaSpBase, IRepositorioResultadoCicloTurma
    {
        public RepositorioResultadoCicloTurma(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<ResultadoCicloTurma> ObterResultadoCicloTurma(string edicao, int areaConhecimentoId, string uadSigla, string escCodigo,
            string turmaCodigo)
        {
            const string query = @"select rct.* 
                                    from [dbo].[ResultadoCicloTurma] rct with (NOLOCK)
                                    where rct.Edicao = @edicao
                                    and rct.AreaConhecimentoID = @areaConhecimentoId
                                    and rct.uad_sigla = @uadSigla
                                    and rct.esc_codigo = @escCodigo
                                    and rct.tur_codigo = @turmaCodigo
                                    order by rct.Edicao desc";

            using var conn = ObterConexaoProvaSp();
            return await conn.QueryFirstOrDefaultAsync<ResultadoCicloTurma>(query,
                new { edicao, areaConhecimentoId, uadSigla, escCodigo = escCodigo.PadLeft(6, '0'), turmaCodigo });
        }

        public async Task<long> IncluirAsync(ResultadoCicloTurma resultado)
        {
            const string query = @"insert into [dbo].[ResultadoCicloTurma]
                                    values(@edicao, @areaConhecimentoId, @uadSigla, @escCodigo, @cicloId, @turmaCodigo,
                                        @turmaId, @valor, @nivelProficienciaId, @totalAlunos, @percentualAbaixoDoBasico,
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

        public async Task<long> AlterarAsync(ResultadoCicloTurma resultado)
        {
            const string query = @"update [dbo].[ResultadoCicloTurma]
                                    set CicloId = @cicloId,
                                        tur_id = @turmaId,
                                        Valor = @valor,
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
                                    and tur_codigo = @turmaCodigo";

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
        
        private static DynamicParameters ObterParametros(ResultadoCicloTurma resultado)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@edicao", resultado.Edicao, DbType.String, ParameterDirection.Input, 10);
            parametros.Add("@areaConhecimentoId", resultado.AreaConhecimentoId, DbType.Int16, ParameterDirection.Input);
            parametros.Add("@uadSigla", resultado.UadSigla, DbType.String, ParameterDirection.Input, 4);
            parametros.Add("@escCodigo", resultado.EscCodigo.PadLeft(6, '0'), DbType.String, ParameterDirection.Input, 20);
            parametros.Add("@cicloId", resultado.CicloId, DbType.Int32, ParameterDirection.Input, 4);
            parametros.Add("@turmaCodigo", resultado.TurmaCodigo, DbType.String, ParameterDirection.Input, 20);
            parametros.Add("@turmaId", resultado.TurmaId, DbType.Int64, ParameterDirection.Input, 8, 19);
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