using Dapper;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Data;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioResultadoSme : RepositorioProvaSpBase, IRepositorioResultadoSme
    {

        public RepositorioResultadoSme(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<ResultadoSme> ObterResultadoSme(string edicao, long areaConhecimentoId, string anoEscolar)
        {
            var query = $@"select
								 Edicao
								,AreaConhecimentoID
								,AnoEscolar
								,Valor
								,TotalAlunos
								,NivelProficienciaID
								,PercentualAbaixoDoBasico
								,PercentualBasico
								,PercentualAdequado
								,PercentualAvancado
								,PercentualAlfabetizado
							from [dbo].[ResultadoSme] with (nolock)
                            where Edicao = @edicao
                              and AreaConhecimentoID = @areaConhecimentoId
                              and AnoEscolar = @anoEscolar
						order by Edicao desc";

            using var conn = ObterConexaoProvaSp();
            return await conn.QueryFirstOrDefaultAsync<ResultadoSme>(query, new { edicao, areaConhecimentoId, anoEscolar });
        }

        public async Task<long> IncluirAsync(ResultadoSme resultado)
        {
            using var conn = ObterConexaoProvaSp();
            try
            {
                var query = $@"insert into [dbo].[ResultadoSme]
								(Edicao
								,AreaConhecimentoID
								,AnoEscolar
								,Valor
								,TotalAlunos
								,NivelProficienciaID
								,PercentualAbaixoDoBasico
								,PercentualBasico
								,PercentualAdequado
								,PercentualAvancado
								,PercentualAlfabetizado)
							values
								(@Edicao
								,@AreaConhecimentoID
								,@AnoEscolar
								,@Valor
								,@TotalAlunos
								,@NivelProficienciaID
								,@PercentualAbaixoDoBasico
								,@PercentualBasico
								,@PercentualAdequado
								,@PercentualAvancado
								,@PercentualAlfabetizado)";

                var parametros = ObterParametros(resultado);
                return await conn.ExecuteAsync(query, parametros);
            }
            catch (Exception ex)
            {
                string log = $@"Incluir {resultado.GetType().Name} -- Erro:{ex.Message} ---- Objeto: {ResultadoPsp.ObterJsonObjetoResultado(resultado)}";
                throw new Exception(log);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<long> AlterarAsync(ResultadoSme resultado)
        {
            using var conn = ObterConexaoProvaSp();
            try
            {
                var query = $@"update [dbo].[ResultadoSme] set									 
									 Valor = @Valor
									,TotalAlunos = @TotalAlunos
									,NivelProficienciaID = @NivelProficienciaID
									,PercentualAbaixoDoBasico = @PercentualAbaixoDoBasico
									,PercentualBasico = @PercentualBasico
									,PercentualAdequado = @PercentualAdequado
									,PercentualAvancado = @PercentualAvancado
									,PercentualAlfabetizado = @PercentualAlfabetizado
								where Edicao = @Edicao
								  and AreaConhecimentoID = @AreaConhecimentoID
								  and AnoEscolar = @AnoEscolar";

                var parametros = ObterParametros(resultado);
                return await conn.ExecuteAsync(query, parametros);

            }
            catch (Exception ex)
            {
                string log = $@"Alterar {resultado.GetType().Name} -- Erro:{ex.Message} ---- Objeto: {ResultadoPsp.ObterJsonObjetoResultado(resultado)}";
                throw new Exception(log);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
        private DynamicParameters ObterParametros(ResultadoSme resultado)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@Edicao", resultado.Edicao, DbType.String, ParameterDirection.Input, 10);
            parametros.Add("@AreaConhecimentoID", resultado.AreaConhecimentoID, DbType.Int16, ParameterDirection.Input);
            parametros.Add("@AnoEscolar", resultado.AnoEscolar, DbType.String, ParameterDirection.Input, 3);
            parametros.Add("@Valor", resultado.Valor, DbType.Decimal, ParameterDirection.Input, null, 6, 3);
            parametros.Add("@TotalAlunos", resultado.TotalAlunos, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@NivelProficienciaID", resultado.NivelProficienciaID, DbType.Int16, ParameterDirection.Input);
            parametros.Add("@PercentualAbaixoDoBasico", resultado.PercentualAbaixoDoBasico, DbType.Decimal, ParameterDirection.Input, null, 6, 2);
            parametros.Add("@PercentualBasico", resultado.PercentualBasico, DbType.Decimal, ParameterDirection.Input, null, 6, 2);
            parametros.Add("@PercentualAdequado", resultado.PercentualAdequado, DbType.Decimal, ParameterDirection.Input, null, 6, 2);
            parametros.Add("@PercentualAvancado", resultado.PercentualAvancado, DbType.Decimal, ParameterDirection.Input, null, 6, 2);
            parametros.Add("@PercentualAlfabetizado", resultado.PercentualAlfabetizado, DbType.Decimal, ParameterDirection.Input, null, 6, 2);
            return parametros;
        }
    }
}
