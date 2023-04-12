using Dapper;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados.Repositorios.ProvaSP
{
    public class RepositorioParticipacaoEscola : RepositorioProvaSpBase, IRepositorioParticipacaoEscola
    {
        public RepositorioParticipacaoEscola(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<ParticipacaoEscola> ObterParticipacaoEscola(string edicao, string uad_sigla, string esc_codigo, string anoEscolar)
        {
            var query = $@"SELECT  [Edicao]
                            ,[uad_sigla]
                            ,[esc_codigo]
                            ,[AnoEscolar]
                            ,[TotalPrevisto]
                            ,[TotalPresente]
                            ,[PercentualParticipacao]   
                           FROM [ProvaSP].[dbo].[ParticipacaoEscola]] WITH (NOLOCK)
                            where Edicao = @edicao
                              and uad_sigla = @uad_sigla     
                              and esc_codigo = REPLICATE('0', 6 - LEN(@esc_codigo)) + RTrim(@esc_codigo)
						      and AnoEscolar = @anoEscolar
                             order by Edicao desc";

            using var conn = ObterConexaoProvaSp();
            return await conn.QueryFirstOrDefaultAsync<ParticipacaoEscola>(query, new { edicao, uad_sigla, esc_codigo, anoEscolar });
        }
        public async Task<long> IncluirAsync(ParticipacaoEscola participacao)
        {
            using var conn = ObterConexaoProvaSp();
            try
            {
                var query = $@"INSERT INTO [dbo].[ParticipacaoEscola]
                                               ([Edicao]
                                               ,[uad_sigla]
                                               ,[esc_codigo]
                                               ,[AnoEscolar]
                                               ,[TotalPrevisto]
                                               ,[TotalPresente]
                                               ,[PercentualParticipacao])
				                    			VALUES
				                    			(@Edicao
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
        public async Task<long> AlterarAsync(ParticipacaoEscola participacao)
        {
            using var conn = ObterConexaoProvaSp();
            try
            {
                var query = @" UPDATE [dbo].[ParticipacaoEscola]
                                 SET [uad_sigla] =  @uad_sigla
                                    ,[esc_codigo] = @esc_codigo
                                    ,[AnoEscolar] = @AnoEscolar
                                    ,[TotalPrevisto] = @TotalPrevisto
                                    ,[TotalPresente] = @TotalPresente
                                    ,[PercentualParticipacao] = @PercentualParticipacao
                               WHERE
                              Edicao = @Edicao
                               and uad_sigla = @uad_sigla
                               and esc_codigo = REPLICATE('0', 6 - LEN(@esc_codigo)) + RTrim(@esc_codigo)
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
        private DynamicParameters ObterParametros(ParticipacaoEscola participacao)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@Edicao", participacao.Edicao, DbType.String, ParameterDirection.Input, 10);
            parametros.Add("@uad_sigla", participacao.UadSigla, DbType.String, ParameterDirection.Input, 4);
            parametros.Add("@esc_codigo", participacao.EscCodigo, DbType.String, ParameterDirection.Input, 20);
            parametros.Add("@AnoEscolar", participacao.AnoEscolar, DbType.String, ParameterDirection.Input, 3);
            parametros.Add("@TotalPrevisto", participacao.TotalPrevisto, DbType.Int32, ParameterDirection.Input, null);
            parametros.Add("@TotalPresente", participacao.TotalPresente, DbType.Int32, ParameterDirection.Input, null);
            parametros.Add("@PercentualParticipacao", participacao.PercentualParticipacao, DbType.Decimal, ParameterDirection.Input, null, 6, 2); ;

            return parametros;
        }
    }
}


