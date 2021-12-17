﻿using Dapper;
using SME.SERAp.Prova.Infra;
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
        /*
        public async Task<ObterAlunoAtivoEolRetornoDto> ObterAlunoAtivoAsync(long alunoRA)
        {

            var query = @"SELECT top 1
	                            aluno.cd_aluno CodigoAluno,
	                            se.sg_resumida_serie as Ano,
                                turesc.cd_tipo_turno as TipoTurno
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
	                            aluno.cd_aluno = @alunoRA
	                            AND matrTurma.cd_situacao_aluno IN (1, 6, 10, 13)
	                            AND e.tp_escola IN (1, 3, 4, 16)";

            using var conn = new SqlConnection(connectionStringOptions.Eol);
            return await conn.QueryFirstOrDefaultAsync<ObterAlunoAtivoEolRetornoDto>(query, new { alunoRA });

        }
        public async Task<AlunoEol> ObterAlunoDetalhePorRa(long alunoRA)
        {

            var query = @"select al.nm_aluno as nome, al.nm_social_aluno as nomeSocial 
                            from aluno al 
			                    where al.cd_aluno = @alunoRA";

            using var conn = new SqlConnection(connectionStringOptions.Eol);
            return await conn.QueryFirstOrDefaultAsync<AlunoEol>(query, new { alunoRA });

        }*/

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
            var query = $@"SELECT 
	                            aluno.cd_aluno CodigoAluno,
                                aluno.nm_aluno as Nome,
                                aluno.dt_nascimento_aluno as DataNascimento,
	                            aluno.cd_sexo_aluno as Sexo,
	                            aluno.nm_social_aluno as NomeSocial,
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
	                            turesc.cd_turma_escola IN ({string.Join(',', turmasCodigo)}) 
	                            AND matrTurma.cd_situacao_aluno IN (1, 6, 10, 13)";

            using var conn = new SqlConnection(connectionStringOptions.Eol);
            return await conn.QueryAsync<AlunoEolDto>(query);
        }
    }
}
