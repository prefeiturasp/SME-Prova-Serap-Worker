using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
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

                return await conn.ExecuteAsync(query, new
                {
                    resultado.Edicao,
                    resultado.AreaConhecimentoID,
                    resultado.AnoEscolar,
                    resultado.Valor,
                    resultado.TotalAlunos,
                    resultado.NivelProficienciaID,
                    resultado.PercentualAbaixoDoBasico,
                    resultado.PercentualBasico,
                    resultado.PercentualAdequado,
                    resultado.PercentualAvancado,
                    resultado.PercentualAlfabetizado
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
