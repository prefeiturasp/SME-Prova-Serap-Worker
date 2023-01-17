using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Dominio.Entidades;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados.Repositorios
{
    public class RepositorioResultadoAluno : IRepositorioResultadoAluno
    {
        private readonly ConnectionStringOptions connectionStringOptions;

        public RepositorioResultadoAluno(ConnectionStringOptions connectionStringOptions)
        {
            this.connectionStringOptions = connectionStringOptions ?? throw new ArgumentNullException(nameof(connectionStringOptions));
        }

        public async Task<ResultadoAluno> ObterProficienciaAluno(string edicao, string alunoMatricula, long turId, long areaConhecimentoId)
        {
            var query = $@"SELECT  Edicao
                                  ,AreaConhecimentoID
                                  ,uad_sigla
                                  ,esc_codigo
                                  ,AnoEscolar
                                  ,tur_codigo
                                  ,tur_id
                                  ,alu_matricula
                                  ,alu_nome
                            --      ,ResultadoLegadoID
                                  ,NivelProficienciaID
                                  ,Valor
                              FROM ResultadoAluno
							where alu_matricula = @alunoMatricula
								and AreaConhecimentoID = @areaConhecimentoId
								and tur_id = @turId
                               and  Edicao = @edicao
						order by Edicao desc";

            using var conn = new SqlConnection(connectionStringOptions.ProvaSP);
            return await conn.QueryFirstOrDefaultAsync<ResultadoAluno>(query, new { alunoMatricula, areaConhecimentoId, turId, edicao });
        }

        public async Task<long> IncluirAsync(ResultadoAluno resultado)
        {
            using var conn = new SqlConnection(connectionStringOptions.ProvaSP);
            try
            {
                var query = $@"INSERT INTO ResultadoAluno
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
                             ,Valor)
                              VALUES
                             ( @Edicao  
                             , @AreaConhecimentoID
                             , @uad_sigla 
                             , @esc_codigo
                             , @AnoEscolar
                             , @tur_codigo
                             , @tur_id
                             , @alu_matricula
                             , @alu_nome
                             , @NivelProficienciaID
                             , CONVERT(decimal(6,3), '{resultado.Valor}'))";

                return await conn.ExecuteAsync(query, new
                {
                    resultado.Edicao,
                    resultado.AreaConhecimentoID,
                    resultado.uad_sigla,
                    resultado.esc_codigo,
                    resultado.AnoEscolar,
                    resultado.tur_codigo,
                    resultado.tur_id,
                    resultado.alu_matricula,
                    resultado.alu_nome,
                    resultado.NivelProficienciaID,
                    resultado.Valor
                });


            }

            finally
            {
                conn.Close();
                conn.Dispose();
            }

        }
    }
}
