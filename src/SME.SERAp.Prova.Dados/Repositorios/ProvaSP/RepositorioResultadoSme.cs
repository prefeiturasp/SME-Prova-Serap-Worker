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

                return await conn.ExecuteAsync(query, parametros);

            }
            catch (Exception ex)
            {
                string log = $@"Erro:{ex.Message} ---- Edicao:{resultado.Edicao}, AreaConhecimentoID:{resultado.AreaConhecimentoID}, 
                                AnoEscolar:{resultado.AnoEscolar}, Valor:{resultado.Valor}, 
                                TotalAlunos:{resultado.TotalAlunos}, NivelProficienciaID:{resultado.NivelProficienciaID}
                                PercentualAbaixoDoBasico:{resultado.PercentualAbaixoDoBasico}, PercentualBasico:{resultado.PercentualBasico}
                                PercentualAdequado:{resultado.PercentualAdequado}, PercentualAvancado:{resultado.PercentualAvancado}";

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
