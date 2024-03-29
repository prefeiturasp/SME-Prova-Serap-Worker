﻿using Dapper;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Data;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados.Repositorios
{
    public class RepositorioResultadoAluno : RepositorioProvaSpBase, IRepositorioResultadoAluno
    {
        public RepositorioResultadoAluno(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions) { }

        public async Task<ResultadoAluno> ObterProficienciaAluno(string edicao, string alunoMatricula, long areaConhecimentoId)
        {
            const string query = @"SELECT Edicao
                                        ,AreaConhecimentoID
                                        ,uad_sigla
                                        ,esc_codigo
                                        ,AnoEscolar
                                        ,tur_codigo
                                        ,tur_id
                                        ,alu_matricula
                                        ,alu_nome
                                        ,NivelProficienciaID
                                        ,Valor
                                        ,REDQ1
                                        ,REDQ2
                                        ,REDQ3
                                        ,REDQ4
                                        ,REDQ5
                                    FROM ResultadoAluno with (nolock)
                                    where alu_matricula = @alunoMatricula
                                    and AreaConhecimentoID = @areaConhecimentoId
                                    and Edicao = @edicao
                                    order by Edicao desc";

            using var conn = ObterConexaoProvaSp();
            return await conn.QueryFirstOrDefaultAsync<ResultadoAluno>(query,
                new { alunoMatricula, areaConhecimentoId, edicao });
        }

        public async Task<long> IncluirAsync(ResultadoAluno resultado)
        {
            using var conn = ObterConexaoProvaSp();
            try
            {
                const string query = @"INSERT INTO ResultadoAluno
                                        (Edicao
                                        ,AreaConhecimentoID
                                        ,uad_sigla
                                        ,esc_codigo
                                        ,AnoEscolar
                                        ,tur_codigo
                                        ,tur_id
                                        ,alu_matricula
                                        ,alu_nome
                                        ,NivelProficienciaID
                                        ,Valor
                                        ,REDQ1
                                        ,REDQ2
                                        ,REDQ3
                                        ,REDQ4
                                        ,REDQ5)
                                        VALUES
                                        ( @Edicao  
                                        , @AreaConhecimentoID
                                        , @uad_sigla 
                                        , REPLICATE('0', 6 - LEN(@esc_codigo)) + RTrim(@esc_codigo)
                                        , @AnoEscolar
                                        , @tur_codigo
                                        , @tur_id
                                        , @alu_matricula
                                        , @alu_nome
                                        , @NivelProficienciaID
                                        , @Valor
                                        , @REDQ1
                                        , @REDQ2
                                        , @REDQ3
                                        , @REDQ4
                                        , @REDQ5)";

                var parametros = ObterParametros(resultado);
                return await conn.ExecuteAsync(query, parametros);
            }
            catch (Exception ex)
            {
                var log = $"Inserir ResultadoAluno -- Erro:{ex.Message} ---- Objeto: {ResultadoPsp.ObterJsonObjetoResultado(resultado)}";
                throw new Exception(log);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<long> AlterarAsync(ResultadoAluno resultado)
        {
            using var conn = ObterConexaoProvaSp();
            try
            {
                const string query = @"update ResultadoAluno set
                                            uad_sigla = @uad_sigla
                                            ,esc_codigo = REPLICATE('0', 6 - LEN(@esc_codigo)) + RTrim(@esc_codigo)
                                            ,AnoEscolar = @AnoEscolar
                                            ,tur_codigo = @tur_codigo
                                            ,tur_id = @tur_id
                                            ,alu_nome = @alu_nome
                                            ,NivelProficienciaID = @NivelProficienciaID
                                            ,Valor = @Valor
                                            ,REDQ1 = @REDQ1
                                            ,REDQ2 = @REDQ2
                                            ,REDQ3 = @REDQ3
                                            ,REDQ4 = @REDQ4
                                            ,REDQ5 = @REDQ5
                                        where alu_matricula = @alu_matricula
                                        and AreaConhecimentoID = @AreaConhecimentoId
                                        and Edicao = @Edicao";

                var parametros = ObterParametros(resultado);
                return await conn.ExecuteAsync(query, parametros);
            }
            catch (Exception ex)
            {
                var log = $"Alterar {resultado.GetType().Name} -- Erro:{ex.Message} ---- Objeto: {ResultadoPsp.ObterJsonObjetoResultado(resultado)}";
                throw new Exception(log);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
        
        private static DynamicParameters ObterParametros(ResultadoAluno resultado)
        {
            var parametros = new DynamicParameters();
            parametros.Add("@Edicao", resultado.Edicao, DbType.String, ParameterDirection.Input, 10);
            parametros.Add("@AreaConhecimentoID", resultado.AreaConhecimentoID, DbType.Int16, ParameterDirection.Input);
            parametros.Add("@uad_sigla", resultado.uad_sigla, DbType.String, ParameterDirection.Input, 4);
            parametros.Add("@esc_codigo", resultado.esc_codigo, DbType.String, ParameterDirection.Input, 20);
            parametros.Add("@AnoEscolar", resultado.AnoEscolar, DbType.String, ParameterDirection.Input, 3);
            parametros.Add("@tur_codigo", resultado.tur_codigo, DbType.String, ParameterDirection.Input, 20);
            parametros.Add("@tur_id", resultado.tur_id, DbType.Int32, ParameterDirection.Input);
            parametros.Add("@alu_matricula", resultado.alu_matricula, DbType.String, ParameterDirection.Input, 50);
            parametros.Add("@alu_nome", resultado.alu_nome, DbType.String, ParameterDirection.Input, 200);
            parametros.Add("@NivelProficienciaID", resultado.NivelProficienciaID, DbType.Int16, ParameterDirection.Input);
            parametros.Add("@Valor", resultado.Valor, DbType.Decimal, ParameterDirection.Input, null, 6, 3);
            parametros.Add("@REDQ1", resultado.REDQ1, DbType.Decimal, ParameterDirection.Input, null, 6, 2);
            parametros.Add("@REDQ2", resultado.REDQ2, DbType.Decimal, ParameterDirection.Input, null, 6, 2);
            parametros.Add("@REDQ3", resultado.REDQ3, DbType.Decimal, ParameterDirection.Input, null, 6, 2);
            parametros.Add("@REDQ4", resultado.REDQ4, DbType.Decimal, ParameterDirection.Input, null, 6, 2);
            parametros.Add("@REDQ5", resultado.REDQ5, DbType.Decimal, ParameterDirection.Input, null, 6, 2);
            return parametros;
        }
    }
}