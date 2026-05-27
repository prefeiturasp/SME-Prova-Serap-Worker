using Microsoft.Data.SqlClient;
using SME.SERAp.Prova.Dominio.Constantes;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
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
            var idsPapStr = string.Join(",", ComponentesCurricularesConstants.IDS_COMPONENTES_CURRICULARES_PAP_NOVO);

            var query = $@"
                            ;WITH mtr_norm AS (
                                SELECT
                                    ROW_NUMBER() OVER (
                                        PARTITION BY matrTurma.cd_matricula
                                        ORDER BY matrTurma.dt_situacao_aluno DESC,
                                                    matrTurma.cd_situacao_aluno
                                    ) AS Linha,
                                    aluno.cd_aluno              AS CodigoAluno,
                                    turesc.cd_turma_escola      AS CodigoTurma,
                                    turesc.an_letivo            AS AnoLetivo,
                                    matrTurma.cd_situacao_aluno AS CodigoSituacaoMatricula,
                                    matrTurma.dt_situacao_aluno AS DataSituacao
                                FROM v_matricula_cotic matricula
                                INNER JOIN v_aluno_cotic aluno
                                    ON matricula.cd_aluno = aluno.cd_aluno
                                INNER JOIN matricula_turma_escola matrTurma
                                    ON matricula.cd_matricula = matrTurma.cd_matricula
                                INNER JOIN turma_escola turesc
                                    ON matrTurma.cd_turma_escola = turesc.cd_turma_escola
                                INNER JOIN escola e
                                    ON turesc.cd_escola = e.cd_escola
                                INNER JOIN serie_turma_escola ste
                                    ON ste.cd_turma_escola = turesc.cd_turma_escola
                                INNER JOIN serie_ensino se
                                    ON se.cd_serie_ensino = ste.cd_serie_ensino
                                WHERE turesc.cd_turma_escola IN ({string.Join(',', turmasCodigo)})
                                    AND se.cd_etapa_ensino NOT IN (14, 18)
                            )
                            SELECT DISTINCT
                                aluno.cd_aluno                          AS CodigoAluno,
                                aluno.nm_aluno                          AS Nome,
                                aluno.dt_nascimento_aluno               AS DataNascimento,
                                aluno.cd_sexo_aluno                     AS Sexo,
                                aluno.nm_social_aluno                   AS NomeSocial,
                                se.sg_resumida_serie                    AS Ano,
                                turesc.cd_tipo_turno                    AS TipoTurno,
                                turesc.cd_turma_escola                  AS TurmaCodigo,
                                turesc.an_letivo                        AS AnoLetivo,
                                matricula.CodigoSituacaoMatricula        AS SituacaoAluno,
                                matricula.DataSituacao,

                                -- Raça
                                trc.dc_raca_cor                         AS Raca,

                                -- AEE: possui ao menos uma deficiência ativa
                                CASE
                                    WHEN EXISTS (
                                        SELECT 1
                                        FROM necessidade_especial_aluno nea WITH (NOLOCK)
                                        INNER JOIN tipo_necessidade_especial tne WITH (NOLOCK)
                                            ON tne.tp_necessidade_especial = nea.tp_necessidade_especial
                                        WHERE nea.cd_aluno = aluno.cd_aluno
                                            AND tne.dt_cancelamento IS NULL
                                            AND (nea.dt_fim IS NULL OR nea.dt_fim >= GETDATE())
                                    ) THEN CAST(1 AS BIT)
                                    ELSE CAST(0 AS BIT)
                                END                                     AS Aee,

                                -- PAP: possui matrícula ativa em turma com componente curricular PAP
                                CASE
                                    WHEN EXISTS (
                                        SELECT 1
                                        FROM v_historico_matricula_cotic hmc WITH (NOLOCK)
                                        INNER JOIN historico_matricula_turma_escola hmte WITH (NOLOCK)
                                            ON hmc.cd_matricula = hmte.cd_matricula
                                        INNER JOIN turma_escola te_pap WITH (NOLOCK)
                                            ON hmte.cd_turma_escola = te_pap.cd_turma_escola
                                        INNER JOIN turma_escola_grade_programa tegp WITH (NOLOCK)
                                            ON tegp.cd_turma_escola = te_pap.cd_turma_escola
                                        INNER JOIN escola_grade teg WITH (NOLOCK)
                                            ON teg.cd_escola_grade = tegp.cd_escola_grade
                                        INNER JOIN grade_componente_curricular pgcc WITH (NOLOCK)
                                            ON pgcc.cd_grade = teg.cd_grade
                                        WHERE hmc.cd_aluno = aluno.cd_aluno
                                            AND te_pap.an_letivo = turesc.an_letivo
                                            AND pgcc.cd_componente_curricular IN ({idsPapStr})
                                            AND te_pap.st_turma_escola IN ('O', 'A', 'C')
                                            AND hmte.cd_situacao_aluno IN (1, 5)
                                    ) THEN CAST(1 AS BIT)
                                    ELSE CAST(0 AS BIT)
                                END                                     AS Pap

                            FROM mtr_norm matricula
                            INNER JOIN v_aluno_cotic aluno
                                ON matricula.CodigoAluno = aluno.cd_aluno
                            INNER JOIN turma_escola turesc
                                ON matricula.CodigoTurma = turesc.cd_turma_escola
                            INNER JOIN escola e
                                ON turesc.cd_escola = e.cd_escola
                            INNER JOIN serie_turma_escola ste
                                ON ste.cd_turma_escola = turesc.cd_turma_escola
                            INNER JOIN serie_ensino se
                                ON se.cd_serie_ensino = ste.cd_serie_ensino

                            -- Raça: LEFT JOIN pois pode ser nulo
                            LEFT JOIN tipo_raca_cor trc
                                ON trc.tp_raca_cor = aluno.tp_raca_cor

                            WHERE matricula.Linha = 1
                                AND turesc.cd_tipo_turma = 1
                                AND CodigoSituacaoMatricula IN (1, 6, 10, 13, 5)
                            ORDER BY aluno.nm_aluno";

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