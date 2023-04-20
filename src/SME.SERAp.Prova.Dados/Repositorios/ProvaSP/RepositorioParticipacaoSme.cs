using Dapper;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioParticipacaoSme : RepositorioProvaSpBase, IRepositorioParticipacaoSme
    {
        public RepositorioParticipacaoSme(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<ParticipacaoSme> ObterParticipacaoSme(string edicao, string anoEscolar)
        {
            var query = $@"SELECT  [Edicao]
                                  ,[AnoEscolar]                                  
                                  ,[TotalPrevisto]
                                  ,[TotalPresente]
                                  ,[PercentualParticipacao]
                              FROM [ProvaSP].[dbo].[ParticipacaoSme] WITH (NOLOCK)
                            where Edicao = @edicao
						      and AnoEscolar = @anoEscolar
                             order by Edicao desc";

            using var conn = ObterConexaoProvaSp();
            return await conn.QueryFirstOrDefaultAsync<ParticipacaoSme>(query, new { edicao, anoEscolar });
        }

        public async Task<long> IncluirAsync(ParticipacaoSme participacao)
        {
            using var conn = ObterConexaoProvaSp();
            try
            {
                var query = $@"INSERT INTO [dbo].[ParticipacaoSme]
                                                  ([Edicao]
                                                  ,[AnoEscolar]                                                  
                                                  ,[TotalPrevisto]
                                                  ,[TotalPresente]
                                                  ,[PercentualParticipacao])
				                    			VALUES
				                    				(@Edicao
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

        public async Task<long> AlterarAsync(ParticipacaoSme participacao)
        {
            using var conn = ObterConexaoProvaSp();
            try
            {
                var query = @" UPDATE [dbo].[ParticipacaoSme]
                                 SET [TotalPrevisto] = @TotalPrevisto
                                    ,[TotalPresente] = @TotalPresente
                                    ,[PercentualParticipacao] = @PercentualParticipacao
                               WHERE
                              Edicao = @Edicao
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

        private DynamicParameters ObterParametros(ParticipacaoSme participacao)
        {
            return participacao.ObterParametrosBase();
        }
    }
}
