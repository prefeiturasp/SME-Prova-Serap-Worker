using Dapper;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados.Repositorios
{
    public class RepositorioResultadoSme : IRepositorioResultadoSme
    {
        private readonly ConnectionStringOptions connectionStringOptions;

        public RepositorioResultadoSme(ConnectionStringOptions connectionStringOptions)
        {
            this.connectionStringOptions = connectionStringOptions ?? throw new ArgumentNullException(nameof(connectionStringOptions));
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
							from [dbo].[ResultadoSme]
                            where Edicao = @edicao
                              and AreaConhecimentoID = @areaConhecimentoId
                              and AnoEscolar = @anoEscolar
						order by Edicao desc";

            using var conn = new SqlConnection(connectionStringOptions.ProvaSP);
            return await conn.QueryFirstOrDefaultAsync<ResultadoSme>(query, new { edicao, areaConhecimentoId, anoEscolar });
        }

        public async Task<long> IncluirAsync(ResultadoSme resultado)
        {
            using var conn = new SqlConnection(connectionStringOptions.ProvaSP);
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

                var parametros = new DynamicParameters();
                parametros.Add("@Edicao", resultado.Edicao, DbType.String, ParameterDirection.Input, 10);
                parametros.Add("@AreaConhecimentoID", resultado.AreaConhecimentoID, DbType.Int16, ParameterDirection.Input);
                parametros.Add("@AnoEscolar", resultado.AnoEscolar, DbType.String, ParameterDirection.Input, 3);
                parametros.Add("@Valor", resultado.Valor.ObterStringDecimalPsp(), DbType.String, ParameterDirection.Input);
                parametros.Add("@TotalAlunos", resultado.TotalAlunos, DbType.Int32, ParameterDirection.Input);
                parametros.Add("@NivelProficienciaID", resultado.NivelProficienciaID, DbType.Int16, ParameterDirection.Input);
                parametros.Add("@PercentualAbaixoDoBasico", resultado.PercentualAbaixoDoBasico.ObterStringDecimalPsp(), DbType.String, ParameterDirection.Input);
                parametros.Add("@PercentualBasico", resultado.PercentualBasico.ObterStringDecimalPsp(), DbType.String, ParameterDirection.Input);
                parametros.Add("@PercentualAdequado", resultado.PercentualAdequado.ObterStringDecimalPsp(), DbType.String, ParameterDirection.Input);
                parametros.Add("@PercentualAvancado", resultado.PercentualAvancado.ObterStringDecimalPsp(), DbType.String, ParameterDirection.Input);
                parametros.Add("@PercentualAlfabetizado", resultado.PercentualAlfabetizado.ObterStringDecimalPsp(), DbType.String, ParameterDirection.Input);

                return await conn.ExecuteAsync(query, parametros);

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
