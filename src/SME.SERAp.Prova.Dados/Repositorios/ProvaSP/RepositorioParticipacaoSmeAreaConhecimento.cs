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
    public class RepositorioParticipacaoSmeAreaConhecimento : RepositorioProvaSpBase, IRepositorioParticipacaoSmeAreaConhecimento
    {
        public RepositorioParticipacaoSmeAreaConhecimento(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<ParticipacaoSmeAreaConhecimento> ObterParticipacaoSmeAreaConhecimento(string edicao, int areaConhecimentoId, string anoEscolar)
        {
            var query = $@"SELECT  [Edicao]
                                  ,[AreaConhecimentoID]
                                  ,[AnoEscolar]                                  
                                  ,[TotalPrevisto]
                                  ,[TotalPresente]
                                  ,[PercentualParticipacao]
                              FROM [ProvaSP].[dbo].[ParticipacaoSmeAreaConhecimento] WITH (NOLOCK)
                            where Edicao = @edicao
						      and AnoEscolar = @anoEscolar
                              and AreaConhecimentoID = @areaConhecimentoId
                             order by Edicao desc";

            using var conn = ObterConexaoProvaSp();
            return await conn.QueryFirstOrDefaultAsync<ParticipacaoSmeAreaConhecimento>(query, new { edicao, areaConhecimentoId, anoEscolar });
        }

        public async Task<long> IncluirAsync(ParticipacaoSmeAreaConhecimento participacao)
        {
            using var conn = ObterConexaoProvaSp();
            try
            {
                var query = $@"INSERT INTO [dbo].[ParticipacaoSmeAreaConhecimento]
                                                  ([Edicao]
                                                  ,[AreaConhecimentoID]
                                                  ,[AnoEscolar]                                                  
                                                  ,[TotalPrevisto]
                                                  ,[TotalPresente]
                                                  ,[PercentualParticipacao])
				                    			VALUES
				                    				(@Edicao
                                                    ,@AreaConhecimentoID
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

        public async Task<long> AlterarAsync(ParticipacaoSmeAreaConhecimento participacao)
        {
            using var conn = ObterConexaoProvaSp();
            try
            {
                var query = @" UPDATE [dbo].[ParticipacaoSmeAreaConhecimento]
                                 SET [TotalPrevisto] = @TotalPrevisto
                                    ,[TotalPresente] = @TotalPresente
                                    ,[PercentualParticipacao] = @PercentualParticipacao
                               WHERE
                              Edicao = @Edicao
                               and AreaConhecimentoID = @AreaConhecimentoID
                               and anoEscolar = @AnoEscolar";


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

        private DynamicParameters ObterParametros(ParticipacaoSmeAreaConhecimento participacao)
        {
            var parametros = participacao.ObterParametrosBase();
            parametros.Add("@AreaConhecimentoID", participacao.AreaConhecimentoID, DbType.Int32, ParameterDirection.Input, 3);
            return parametros;
        }
    }
}
