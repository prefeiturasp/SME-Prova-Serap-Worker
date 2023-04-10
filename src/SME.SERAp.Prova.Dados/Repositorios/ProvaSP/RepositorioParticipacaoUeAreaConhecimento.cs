using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Data;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioParticipacaoUeAreaConhecimento : RepositorioProvaSpBase, IRepositorioParticipacaoUeAreaConhecimento
    {
        public RepositorioParticipacaoUeAreaConhecimento(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<ParticipacaoUeAreaConhecimento> ObterParticipacaoUeAreaConhecimento(string edicao, string uad_sigla, int areaConhecimentoId, string esc_codigo, string anoEscolar)
        {
            var query = $@"SELECT  [Edicao]
                                  ,[AreaConhecimentoID]
                                  ,[uad_sigla]
                                  ,[esc_codigo]
                                  ,[AnoEscolar]                                  
                                  ,[TotalPrevisto]
                                  ,[TotalPresente]
                                  ,[PercentualParticipacao]
                              FROM [ProvaSP].[dbo].[ParticipacaoEscolaAreaConhecimento] WITH (NOLOCK)
                            where Edicao = @edicao
                              and uad_sigla = @uad_sigla     
                              and esc_codigo = REPLICATE('0', 6 - LEN(@esc_codigo)) + RTrim(@esc_codigo)
						      and AnoEscolar = @anoEscolar
                              and AreaConhecimentoID = @areaConhecimentoId
                             order by Edicao desc";

            using var conn = ObterConexaoProvaSp();
            return await conn.QueryFirstOrDefaultAsync<ParticipacaoUeAreaConhecimento>(query, new { edicao, areaConhecimentoId, uad_sigla, esc_codigo, anoEscolar });
        }

        public async Task<long> IncluirAsync(ParticipacaoUeAreaConhecimento participacao)
        {
            using var conn = ObterConexaoProvaSp();
            try
            {
                var query = $@"INSERT INTO [dbo].[ParticipacaoEscolaAreaConhecimento]
                                                  ([Edicao]
                                                  ,[AreaConhecimentoID]
                                                  ,[uad_sigla]
                                                  ,[esc_codigo]
                                                  ,[AnoEscolar]                                                  
                                                  ,[TotalPrevisto]
                                                  ,[TotalPresente]
                                                  ,[PercentualParticipacao])
				                    			VALUES
				                    				(@Edicao
                                                    ,@AreaConhecimentoID
				                    				,@uad_sigla
                                                    ,REPLICATE('0', 6 - LEN(@esc_codigo)) + RTrim(@esc_codigo)
				                    				,@AnoEscolar                                                    
				                    				,@TotalPrevisto
				                    				,@TotalPresente
				                    				,@PercentualParticipacao)";

                var parametros = ObterParametros(participacao);
                return await conn.ExecuteAsync(query, parametros);

            }
            catch (Exception ex)
            {
                string log = $@"Erro:{ex.Message} ---- Objeto: {ResultadoPsp.ObterJsonObjetoResultado(participacao)}";
                throw new Exception(log);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<long> AlterarAsync(ParticipacaoUeAreaConhecimento participacao)
        {
            using var conn = ObterConexaoProvaSp();
            try
            {
                var query = $@"
                                UPDATE [dbo].[ParticipacaoEscolaAreaConhecimento]
                                   SET [AreaConhecimentoID] =  @AreaConhecimentoID
                                      ,[uad_sigla] =  @uad_sigla
                                      ,[esc_codigo] = @esc_codigo
                                      ,[AnoEscolar] = @AnoEscolar                                      
                                      ,[TotalPrevisto] = @TotalPrevisto
                                      ,[TotalPresente] = @TotalPresente
                                      ,[PercentualParticipacao] = @PercentualParticipacao
                                 WHERE
								 Edicao = @Edicao
                                 and AreaConhecimentoID = @AreaConhecimentoID
                                 and uad_sigla = @uad_sigla
                                 and esc_codigo = REPLICATE('0', 6 - LEN(@esc_codigo)) + RTrim(@esc_codigo)
                                 and anoEscolar = @AnoEscolar
                                 and tur_codigo = @tur_codigo";

                var parametros = ObterParametros(participacao);
                return await conn.ExecuteAsync(query, parametros);

            }
            catch (Exception ex)
            {
                string log = $@"Alterar {participacao.GetType().Name} -- Erro:{ex.Message} ---- Objeto: {ResultadoPsp.ObterJsonObjetoResultado(participacao)}";
                throw new Exception(log);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private DynamicParameters ObterParametros(ParticipacaoUeAreaConhecimento participacao)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@Edicao", participacao.Edicao, DbType.String, ParameterDirection.Input, 10);
            parametros.Add("@AreaConhecimentoID", participacao.AreaConhecimentoID, DbType.Int32, ParameterDirection.Input, 3);
            parametros.Add("@uad_sigla", participacao.UadSigla, DbType.String, ParameterDirection.Input, 4);
            parametros.Add("@esc_codigo", participacao.EscCodigo, DbType.String, ParameterDirection.Input, 20);
            parametros.Add("@AnoEscolar", participacao.AnoEscolar, DbType.String, ParameterDirection.Input, 3);           
            parametros.Add("@TotalPrevisto", participacao.TotalPrevisto, DbType.Int32, ParameterDirection.Input, null);
            parametros.Add("@TotalPresente", participacao.TotalPresente, DbType.Int32, ParameterDirection.Input, null);
            parametros.Add("@PercentualParticipacao", participacao.PercentualParticipacao, DbType.Decimal, ParameterDirection.Input, null, 6, 2);
            return parametros;
        }
    }
}
