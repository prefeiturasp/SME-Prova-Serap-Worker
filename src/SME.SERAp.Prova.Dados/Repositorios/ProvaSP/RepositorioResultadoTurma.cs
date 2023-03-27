using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Data;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioResultadoTurma : RepositorioProvaSpBase, IRepositorioResultadoTurma
    {
        public RepositorioResultadoTurma(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<ResultadoTurma> ObterResultadoTurma(string edicao, long areaConhecimentoId, string esc_codigo, string tur_codigo)
        {
            var query = $@"select
								 Edicao
								,AreaConhecimentoID
                                ,uad_sigla as UadSigla
                                ,esc_codigo as EscCodigo
								,AnoEscolar
                                ,tur_codigo as TurCodigo
                                ,tur_id as TurId
								,Valor
								,TotalAlunos
								,NivelProficienciaID
								,PercentualAbaixoDoBasico
								,PercentualBasico
								,PercentualAdequado
								,PercentualAvancado
								,PercentualAlfabetizado
							from [dbo].[ResultadoTurma] with (NOLOCK)
                            where Edicao = @edicao
                              and AreaConhecimentoID = @areaConhecimentoId
                              and tur_codigo = @tur_codigo
                              and esc_codigo = REPLICATE('0', 6 - LEN(@esc_codigo)) + RTrim(@esc_codigo)
						order by Edicao desc";

            using var conn = ObterConexaoProvaSp();
            return await conn.QueryFirstOrDefaultAsync<ResultadoTurma>(query, new { edicao, areaConhecimentoId, esc_codigo, tur_codigo });
        }

        public async Task<long> IncluirAsync(ResultadoTurma resultado)
        {
            using var conn = ObterConexaoProvaSp();
            try
            {
                var query = $@"insert into [dbo].[ResultadoTurma]
								(Edicao
								,AreaConhecimentoID
                                ,uad_sigla
                                ,esc_codigo
								,AnoEscolar
                                ,tur_codigo
                                ,tur_id
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
                                ,@UadSigla
                                ,REPLICATE('0', 6 - LEN(@EscCodigo)) + RTrim(@EscCodigo)
								,@AnoEscolar
                                ,@TurCodigo
                                ,@TurId
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
                string log = $@"Erro:{ex.Message} ---- Objeto: {ResultadoPsp.ObterJsonObjetoResultado(resultado)}";
                throw new Exception(log);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<long> AlterarAsync(ResultadoTurma resultado)
        {
            using var conn = ObterConexaoProvaSp();
            try
            {
                var query = $@"update [dbo].[ResultadoTurma] set
                                 uad_sigla = @UadSigla
								,AnoEscolar = @AnoEscolar
                                ,tur_id = @TurId
								,Valor = @Valor
								,TotalAlunos = @TotalAlunos
								,NivelProficienciaID = @NivelProficienciaID
								,PercentualAbaixoDoBasico = @PercentualAbaixoDoBasico
								,PercentualBasico = @PercentualBasico
								,PercentualAdequado = @PercentualAdequado
								,PercentualAvancado = @PercentualAvancado
								,PercentualAlfabetizado = @PercentualAlfabetizado
								where Edicao = @Edicao
                                and AreaConhecimentoID = @AreaConhecimentoID
                                and tur_codigo = @TurCodigo
                                and esc_codigo = REPLICATE('0', 6 - LEN(@EscCodigo)) + RTrim(@EscCodigo)";

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
        private DynamicParameters ObterParametros(ResultadoTurma resultado)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@Edicao", resultado.Edicao, DbType.String, ParameterDirection.Input, 10);
            parametros.Add("@AreaConhecimentoID", resultado.AreaConhecimentoID, DbType.Int16, ParameterDirection.Input);
            parametros.Add("@UadSigla", resultado.UadSigla, DbType.String, ParameterDirection.Input, 4);
            parametros.Add("@EscCodigo", resultado.EscCodigo, DbType.String, ParameterDirection.Input, 20);
            parametros.Add("@AnoEscolar", resultado.AnoEscolar, DbType.String, ParameterDirection.Input, 3);
            parametros.Add("@TurCodigo", resultado.TurCodigo, DbType.String, ParameterDirection.Input, 20);
            parametros.Add("@TurId", resultado.TurId, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@Valor", resultado.Valor, DbType.Decimal, ParameterDirection.Input, null, 6, 3);
            parametros.Add("@TotalAlunos", resultado.TotalAlunos, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@NivelProficienciaID", resultado.NivelProficienciaID, DbType.Int16, ParameterDirection.Input);
            parametros.Add("@PercentualAbaixoDoBasico", resultado.PercentualAbaixoDoBasico ?? 0, DbType.Decimal, ParameterDirection.Input, null, 6, 2);
            parametros.Add("@PercentualBasico", resultado.PercentualBasico ?? 0, DbType.Decimal, ParameterDirection.Input, null, 6, 2);
            parametros.Add("@PercentualAdequado", resultado.PercentualAdequado ?? 0, DbType.Decimal, ParameterDirection.Input, null, 6, 2);
            parametros.Add("@PercentualAvancado", resultado.PercentualAvancado ?? 0, DbType.Decimal, ParameterDirection.Input, null, 6, 2);
            parametros.Add("@PercentualAlfabetizado", resultado.PercentualAlfabetizado ?? 0, DbType.Decimal, ParameterDirection.Input, null, 6, 2);
            return parametros;
        }
    }
}
