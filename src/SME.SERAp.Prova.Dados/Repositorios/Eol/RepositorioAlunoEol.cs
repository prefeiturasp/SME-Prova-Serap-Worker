﻿using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioAlunoEol : IRepositorioAlunoEol
    {
        private readonly ConnectionStringOptions connectionStringOptions;

        public RepositorioAlunoEol(ConnectionStringOptions connectionStringOptions)
        {
            this.connectionStringOptions = connectionStringOptions ?? throw new ArgumentNullException(nameof(connectionStringOptions));
        }
        
        public async Task<IEnumerable<AlunoEolDto>> ObterAlunosPorTurmaCodigoAsync(long turmaCodigo)
        {
            var query = @"SELECT 
	                            aluno.cd_aluno CodigoAluno,
                                aluno.nm_aluno as Nome,
	                            se.sg_resumida_serie as Ano,
	                            turesc.cd_tipo_turno as TipoTurno,
	                            turesc.cd_turma_escola as TurmaCodigo,
	                            turesc.an_letivo as AnoLetivo,
	                            matrTurma.cd_situacao_aluno as SituacaoAluno
                            FROM
	                            v_matricula_cotic matricula
                            INNER JOIN v_aluno_cotic aluno ON
	                            matricula.cd_aluno = aluno.cd_aluno
                            INNER JOIN matricula_turma_escola matrTurma ON
	                            matricula.cd_matricula = matrTurma.cd_matricula
                            INNER JOIN turma_escola turesc ON
	                            matrTurma.cd_turma_escola = turesc.cd_turma_escola
                            INNER JOIN escola e ON
	                            turesc.cd_escola = e.cd_escola
                            INNER JOIN serie_turma_escola ste ON
								ste.cd_turma_escola = turesc.cd_turma_escola
							INNER JOIN serie_ensino se ON 
								se.cd_serie_ensino = ste.cd_serie_ensino
                            WHERE
	                            turesc.cd_turma_escola = @turmaCodigo
	                            AND matrTurma.cd_situacao_aluno IN (1, 6, 10, 13)";

            using var conn = new SqlConnection(connectionStringOptions.Eol);
            return await conn.QueryAsync<AlunoEolDto>(query, new { turmaCodigo });
        }

        public async Task<IEnumerable<AlunoEolDto>> ObterAlunosPorTurmasCodigoAsync(long[] turmasCodigo)
        {            
            var query = $@";with mtr_norm as (
								SELECT
									ROW_NUMBER() OVER(PARTITION BY matrTurma.cd_matricula ORDER BY matrTurma.dt_situacao_aluno DESC, matrTurma.cd_situacao_aluno) AS Linha,
									aluno.cd_aluno as CodigoAluno,
									turesc.cd_turma_escola as CodigoTurma,
									turesc.an_letivo as AnoLetivo,
									matrTurma.cd_situacao_aluno as CodigoSituacaoMatricula,
									matrTurma.dt_situacao_aluno as DataSituacao
								FROM
									v_matricula_cotic matricula
								INNER JOIN v_aluno_cotic aluno ON
									matricula.cd_aluno = aluno.cd_aluno
								INNER JOIN matricula_turma_escola matrTurma ON
									matricula.cd_matricula = matrTurma.cd_matricula
								INNER JOIN turma_escola turesc ON
									matrTurma.cd_turma_escola = turesc.cd_turma_escola
								INNER JOIN escola e ON
									turesc.cd_escola = e.cd_escola
								INNER JOIN serie_turma_escola ste ON
									ste.cd_turma_escola = turesc.cd_turma_escola
								INNER JOIN serie_ensino se ON
									se.cd_serie_ensino = ste.cd_serie_ensino							
								WHERE turesc.cd_turma_escola in ({string.Join(',', turmasCodigo)})
								  and se.cd_etapa_ensino not in (14, 18)
							)
							SELECT distinct
								aluno.cd_aluno CodigoAluno,
								aluno.nm_aluno as Nome,
								aluno.dt_nascimento_aluno as DataNascimento,
								aluno.cd_sexo_aluno as Sexo,
								aluno.nm_social_aluno as NomeSocial,
								se.sg_resumida_serie as Ano,
								turesc.cd_tipo_turno as TipoTurno,
								turesc.cd_turma_escola as TurmaCodigo,
								turesc.an_letivo as AnoLetivo,
								matricula.CodigoSituacaoMatricula as SituacaoAluno,
								matricula.DataSituacao
							FROM
								mtr_norm matricula 
							INNER JOIN v_aluno_cotic aluno ON
								matricula.CodigoAluno = aluno.cd_aluno
							INNER JOIN turma_escola turesc ON
								matricula.CodigoTurma = turesc.cd_turma_escola
							INNER JOIN escola e ON
								turesc.cd_escola = e.cd_escola
							INNER JOIN serie_turma_escola ste ON
								ste.cd_turma_escola = turesc.cd_turma_escola
							INNER JOIN serie_ensino se ON 
								se.cd_serie_ensino = ste.cd_serie_ensino
							WHERE matricula.Linha = 1
							  and turesc.cd_tipo_turma = 1
							  and CodigoSituacaoMatricula in (1, 6, 10, 13, 5) -- Alunos que podem acessar o serap
							order by aluno.nm_aluno";

            await using var conn = new SqlConnection(connectionStringOptions.Eol);
            return await conn.QueryAsync<AlunoEolDto>(query);
        }

        public async Task<IEnumerable<int>> ObterAlunoDeficienciaPorAlunoRa(long alunoRa)
        {
            var query = $@"select tne.tp_necessidade_especial 
                            from necessidade_especial_aluno nea
                            inner join tipo_necessidade_especial tne 
                                on tne.tp_necessidade_especial = nea.tp_necessidade_especial
                            where nea.cd_aluno = @alunoRa
                                and tne.dt_cancelamento is null
								and (nea.dt_fim is null or nea.dt_fim >= GETDATE())";

            using var conn = new SqlConnection(connectionStringOptions.Eol);
            return await conn.QueryAsync<int>(query, new { alunoRa });
        }
    }
}
