using Dapper;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using SME.SERAp.Prova.Infra;
using System;
using System.Data;
using System.Threading.Tasks;
using SME.SERAp.Prova.Dominio.Entidades.ProficienciaPsp;
using SME.SERAp.Prova.Dados.Interfaces;

namespace SME.SERAp.Prova.Dados.Repositorios.ProvaSP
{
    public class RepositorioResultadoCicloDre : RepositorioProvaSpBase, IRepositorioResultadoCicloDre
    {
        public RepositorioResultadoCicloDre(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<ResultadoCicloDre> ObterResultadoCicloDre(string edicao, int areaConhecimentoId, string dreSigla, int cicloId)
        {
            const string query = @"select rcs.* 
                                    from [dbo].[ResultadoCicloDre] rcs with (NOLOCK)
                                    where rcs.Edicao = @edicao
                                    and rcs.AreaConhecimentoID = @areaConhecimentoId
                                    and rcs.CicloID = @cicloId
                                    and rcs.uad_sigla = @dreSigla
                                    order by rcs.Edicao desc";

            using var conn = ObterConexaoProvaSp();
            return await conn.QueryFirstOrDefaultAsync<ResultadoCicloDre>(query,
                new { edicao, areaConhecimentoId, cicloId, dreSigla });
        }

        public async Task<long> IncluirAsync(ResultadoCicloDre resultado)
        {
            const string query = @"insert into [dbo].[ResultadoCicloDre]
                                    values(@edicao, @areaConhecimentoId, @dreSigla, @cicloId, @valor, @totalAlunos, 
                                        @nivelProficienciaId, @percentualAbaixoDoBasico, @percentualBasico,
                                        @percentualAdequado, @percentualAvancado, @percentualAlfabetizado)";

            using var conn = ObterConexaoProvaSp();
            try
            {
                var parametros = ObterParametros(resultado);
                return await conn.ExecuteAsync(query, parametros);
            }
            catch (Exception e)
            {
                var log = $@"Incluir {resultado.GetType().Name} -- Erro:{e.Message} ---- Objeto: {ResultadoPsp.ObterJsonObjetoResultado(resultado)}";
                throw new Exception(log);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<long> AlterarAsync(ResultadoCicloDre resultado)
        {
            const string query = @"update [dbo].[ResultadoCicloDre]
                                    set Valor = @valor,
                                        TotalAlunos = @totalAlunos,
                                        NivelProficienciaID = @nivelProficienciaId,
                                        PercentualAbaixoDoBasico = @percentualAbaixoDoBasico,
                                        PercentualBasico = @percentualBasico,
                                        PercentualAdequado = @percentualAdequado,
                                        PercentualAvancado = @percentualAvancado,
                                        PercentualAlfabetizado = @percentualAlfabetizado
                                  
                                    where Edicao = @edicao
                                    and AreaConhecimentoID = @areaConhecimentoId
                                    and CicloID = @cicloId
                                    and uad_sigla = @dreSigla";


            var conn = ObterConexaoProvaSp();
            try
            {
                var parametros = ObterParametros(resultado);
                return await conn.ExecuteAsync(query, parametros);
            }
            catch (Exception e)
            {
                var log = $@"Alterar {resultado.GetType().Name} -- Erro:{e.Message} ---- Objeto: {ResultadoPsp.ObterJsonObjetoResultado(resultado)}";
                throw new Exception(log);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        private static DynamicParameters ObterParametros(ResultadoCicloDre resultado)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@edicao", resultado.Edicao, DbType.String, ParameterDirection.Input, 10);
            parametros.Add("@areaConhecimentoId", resultado.AreaConhecimentoId, DbType.Int16, ParameterDirection.Input);
            parametros.Add("@dreSigla", resultado.DreSigla, DbType.String, ParameterDirection.Input);
            parametros.Add("@cicloId", resultado.CicloId, DbType.Int32, ParameterDirection.Input, 4);
            parametros.Add("@valor", resultado.Valor, DbType.Decimal, ParameterDirection.Input, 5, 6, 3);
            parametros.Add("@totalAlunos", resultado.TotalAlunos, DbType.Int32, ParameterDirection.Input, 4);
            parametros.Add("@nivelProficienciaId", resultado.NivelProficienciaId, DbType.Int16, ParameterDirection.Input);
            parametros.Add("@percentualAbaixoDoBasico", resultado.PercentualAbaixoDoBasico, DbType.Decimal, ParameterDirection.Input, 5, 6, 2);
            parametros.Add("@percentualBasico", resultado.PercentualBasico, DbType.Decimal, ParameterDirection.Input, 5, 6, 2);
            parametros.Add("@percentualAdequado", resultado.PercentualAdequado, DbType.Decimal, ParameterDirection.Input, 5, 6, 2);
            parametros.Add("@percentualAvancado", resultado.PercentualAvancado, DbType.Decimal, ParameterDirection.Input, 5, 6, 2);
            parametros.Add("@percentualAlfabetizado", resultado.PercentualAlfabetizado, DbType.Decimal, ParameterDirection.Input, 5, 6, 2);
            return parametros;
        }
    }
}

