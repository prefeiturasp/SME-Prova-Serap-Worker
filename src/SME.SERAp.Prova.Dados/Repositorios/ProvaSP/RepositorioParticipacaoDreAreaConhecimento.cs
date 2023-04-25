using Dapper;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Data;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados.Repositorios.ProvaSP
{
    public class RepositorioParticipacaoDreAreaConhecimento : RepositorioProvaSpBase, IRepositorioParticipacaoDreAreaConhecimento
    {
        public RepositorioParticipacaoDreAreaConhecimento(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<ParticipacaoDreAreaConhecimento> ObterParticipacaoDreAreaConhecimento(string edicao, int areaConhecimentoId, string uad_sigla, string anoEscolar)
        {
            var query = $@"SELECT  [Edicao]
                                  ,[AreaConhecimentoID]
                                  ,[uad_sigla]
                                  ,[AnoEscolar]                                  
                                  ,[TotalPrevisto]
                                  ,[TotalPresente]
                                  ,[PercentualParticipacao]
                              FROM [ProvaSP].[dbo].[ParticipacaoDreAreaConhecimento] WITH (NOLOCK)
                            where Edicao = @edicao
                              and uad_sigla = @uad_sigla
						      and AnoEscolar = @anoEscolar
                              and AreaConhecimentoID = @areaConhecimentoId
                             order by Edicao desc";

            using var conn = ObterConexaoProvaSp();
            return await conn.QueryFirstOrDefaultAsync<ParticipacaoDreAreaConhecimento>(query, new { edicao, areaConhecimentoId, uad_sigla, anoEscolar });
        }
        public async Task<long> IncluirAsync(ParticipacaoDreAreaConhecimento participacao)
        {
            using var conn = ObterConexaoProvaSp();
            try
            {
                var query = $@"INSERT INTO [dbo].[ParticipacaoDreAreaConhecimento]
                                                  ([Edicao]
                                                  ,[AreaConhecimentoID]
                                                  ,[uad_sigla]
                                                  ,[AnoEscolar]                                                  
                                                  ,[TotalPrevisto]
                                                  ,[TotalPresente]
                                                  ,[PercentualParticipacao])
				                    			VALUES
				                    				(@Edicao
                                                    ,@AreaConhecimentoID
				                    				,@uad_sigla
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
        public async Task<long> AlterarAsync(ParticipacaoDreAreaConhecimento participacao)
        {
            using var conn = ObterConexaoProvaSp();
            try
            {
                var query = @" UPDATE [dbo].[ParticipacaoDreAreaConhecimento]
                                 SET [uad_sigla] =  @uad_sigla
                                    ,[AnoEscolar] = @AnoEscolar                                     
                                    ,[TotalPrevisto] = @TotalPrevisto
                                    ,[TotalPresente] = @TotalPresente
                                    ,[PercentualParticipacao] = @PercentualParticipacao
                               WHERE
                              Edicao = @Edicao
                               and AreaConhecimentoID = @AreaConhecimentoID
                               and uad_sigla = @uad_sigla
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
        private DynamicParameters ObterParametros(ParticipacaoDreAreaConhecimento participacao)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@Edicao", participacao.Edicao, DbType.String, ParameterDirection.Input, 10);
            parametros.Add("@AreaConhecimentoID", participacao.AreaConhecimentoID, DbType.Int32, ParameterDirection.Input, 3);
            parametros.Add("@uad_sigla", participacao.UadSigla, DbType.String, ParameterDirection.Input, 4);
            parametros.Add("@AnoEscolar", participacao.AnoEscolar, DbType.String, ParameterDirection.Input, 3);
            parametros.Add("@TotalPrevisto", participacao.TotalPrevisto, DbType.Int32, ParameterDirection.Input, null);
            parametros.Add("@TotalPresente", participacao.TotalPresente, DbType.Int32, ParameterDirection.Input, null);
            parametros.Add("@PercentualParticipacao", participacao.PercentualParticipacao, DbType.Decimal, ParameterDirection.Input, null, 6, 2);

            return parametros;
        }
    }
}
