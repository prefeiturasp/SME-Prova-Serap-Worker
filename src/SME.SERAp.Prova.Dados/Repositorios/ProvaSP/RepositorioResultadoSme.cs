using Dapper;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

                //var parametros = new DynamicParameters();
                //parametros.Add("@Edicao", resultado.Edicao, DbType.VarChar, ParameterDirection.Input, 10);
                //parametros.Add("@AreaConhecimentoID", resultado.AreaConhecimentoID, DbType.Int16, ParameterDirection.Input);
                //parametros.Add("@AnoEscolar", resultado.AnoEscolar, DbType.String, ParameterDirection.Input, 3);
                //parametros.Add("@Valor", resultado.Valor.ObterStringDecimalPsp(), DbType.String, ParameterDirection.Input);
                //parametros.Add("@TotalAlunos", resultado.TotalAlunos, DbType.Int32, ParameterDirection.Input);
                //parametros.Add("@NivelProficienciaID", resultado.NivelProficienciaID, DbType.Int16, ParameterDirection.Input);
                //parametros.Add("@PercentualAbaixoDoBasico", resultado.PercentualAbaixoDoBasico.ObterStringDecimalPsp(), DbType.String, ParameterDirection.Input);
                //parametros.Add("@PercentualBasico", resultado.PercentualBasico.ObterStringDecimalPsp(), DbType.String, ParameterDirection.Input);
                //parametros.Add("@PercentualAdequado", resultado.PercentualAdequado.ObterStringDecimalPsp(), DbType.String, ParameterDirection.Input);
                //parametros.Add("@PercentualAvancado", resultado.PercentualAvancado.ObterStringDecimalPsp(), DbType.String, ParameterDirection.Input);
                //parametros.Add("@PercentualAlfabetizado", resultado.PercentualAlfabetizado.ObterStringDecimalPsp(), DbType.String, ParameterDirection.Input);

                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                //var parametros = new List<SqlParameter>();
                //command.Parameters.Add(new SqlParameter { ParameterName = "@Edicao", Value = resultado.Edicao, SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input });
                //command.Parameters.Add(new SqlParameter { ParameterName = "@AreaConhecimentoID", Value = resultado.AreaConhecimentoID, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
                //command.Parameters.Add(new SqlParameter { ParameterName = "@AnoEscolar", Value = resultado.AnoEscolar, SqlDbType = SqlDbType.VarChar, Direction = ParameterDirection.Input });
                //command.Parameters.Add(new SqlParameter { ParameterName = "@Valor", Value = resultado.Valor, SqlDbType = SqlDbType.Decimal, Direction = ParameterDirection.Input });
                //command.Parameters.Add(new SqlParameter { ParameterName = "@TotalAlunos", Value = resultado.TotalAlunos, SqlDbType = SqlDbType.BigInt, Direction = ParameterDirection.Input });
                //command.Parameters.Add(new SqlParameter { ParameterName = "@NivelProficienciaID", Value = resultado.NivelProficienciaID, SqlDbType = SqlDbType.Int, Direction = ParameterDirection.Input });
                //command.Parameters.Add(new SqlParameter { ParameterName = "@PercentualAbaixoDoBasico", Value = resultado.PercentualAbaixoDoBasico, SqlDbType = SqlDbType.Decimal, Direction = ParameterDirection.Input });
                //command.Parameters.Add(new SqlParameter { ParameterName = "@PercentualBasico", Value = resultado.PercentualBasico, SqlDbType = SqlDbType.Decimal, Direction = ParameterDirection.Input });
                //command.Parameters.Add(new SqlParameter { ParameterName = "@PercentualAdequado", Value = resultado.PercentualAdequado, SqlDbType = SqlDbType.Decimal, Direction = ParameterDirection.Input });
                //command.Parameters.Add(new SqlParameter { ParameterName = "@PercentualAvancado", Value = resultado.PercentualAvancado, SqlDbType = SqlDbType.Decimal, Direction = ParameterDirection.Input });
                //command.Parameters.Add(new SqlParameter { ParameterName = "@PercentualAlfabetizado", Value = DBNull.Value, SqlDbType = SqlDbType.Decimal, Direction = ParameterDirection.Input });

                command.Parameters.AddWithValue("@Edicao", resultado.Edicao);
                command.Parameters.AddWithValue("@AreaConhecimentoID", resultado.AreaConhecimentoID);
                command.Parameters.AddWithValue("@AnoEscolar", resultado.AnoEscolar);
                command.Parameters.AddWithValue("@Valor", resultado.Valor);
                command.Parameters.AddWithValue("@TotalAlunos", resultado.TotalAlunos);
                command.Parameters.AddWithValue("@NivelProficienciaID", resultado.NivelProficienciaID);
                command.Parameters.AddWithValue("@PercentualAbaixoDoBasico", resultado.PercentualAbaixoDoBasico);
                command.Parameters.AddWithValue("@PercentualBasico", resultado.PercentualBasico);
                command.Parameters.AddWithValue("@PercentualAdequado", resultado.PercentualAdequado);
                command.Parameters.AddWithValue("@PercentualAvancado", resultado.PercentualAvancado);
                command.Parameters.AddWithValue("@PercentualAlfabetizado", DBNull.Value);


                return await command.ExecuteNonQueryAsync();

                //return await conn.ExecuteAsync(query, parametros);

            }
            catch (Exception ex)
            {
                string log = $@"Erro:{ex.Message} ---- Edicao:{resultado.Edicao}, AreaConhecimentoID:{resultado.AreaConhecimentoID}, 
                                AnoEscolar:{resultado.AnoEscolar}, Valor:{resultado.Valor}, 
                                TotalAlunos:{resultado.TotalAlunos}, NivelProficienciaID:{resultado.NivelProficienciaID}
                                PercentualAbaixoDoBasico:{resultado.PercentualAbaixoDoBasico}, PercentualBasico:{resultado.PercentualBasico}
                                PercentualAdequado:{resultado.PercentualAdequado}, PercentualAvancado{resultado.PercentualAvancado}";
                throw new Exception(log);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
