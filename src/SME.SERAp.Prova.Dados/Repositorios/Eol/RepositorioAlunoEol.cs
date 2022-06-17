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
								select ROW_NUMBER() OVER(PARTITION BY amn.CodigoMatricula ORDER BY amn.DataSituacao DESC) AS Linha,
									amn.CodigoAluno,
									amn.CodigoTurma,
									amn.AnoLetivo,
									amn.CodigoSituacaoMatricula,
									amn.DataSituacao
								from alunos_matriculas_norm amn
								where amn.CodigoTurma in ({string.Join(',', turmasCodigo)})
							)

							SELECT 
								aluno.cd_aluno CodigoAluno,
								aluno.nm_aluno as Nome,
								aluno.dt_nascimento_aluno as DataNascimento,
								aluno.cd_sexo_aluno as Sexo,
								aluno.nm_social_aluno as NomeSocial,
								se.sg_resumida_serie as Ano,
								turesc.cd_tipo_turno as TipoTurno,
								turesc.cd_turma_escola as TurmaCodigo,
								turesc.an_letivo as AnoLetivo,
								matricula.CodigoSituacaoMatricula as SituacaoAluno
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
							and CodigoSituacaoMatricula in (1, 5, 6, 10, 13)
							order by aluno.nm_aluno";

            using var conn = new SqlConnection(connectionStringOptions.Eol);
            return await conn.QueryAsync<AlunoEolDto>(query);
        }

        public async Task<IEnumerable<int>> ObterAlunoDeficienciaPorAlunoRa(long alunoRa)
        {
            var query = $@"select tne.tp_necessidade_especial 
                            from necessidade_especial_aluno nea
                            inner join tipo_necessidade_especial tne 
                                on tne.tp_necessidade_especial = nea.tp_necessidade_especial
                            where nea.cd_aluno = @alunoRa
                                and tne.dt_cancelamento is null";

            using var conn = new SqlConnection(connectionStringOptions.Eol);
            return await conn.QueryAsync<int>(query, new { alunoRa });
        }
    }
}
