using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioTurmaEol : IRepositorioTurmaEol
    {

        private readonly ConnectionStringOptions connectionStringOptions;

        public RepositorioTurmaEol(ConnectionStringOptions connectionStringOptions)
        {
            this.connectionStringOptions = connectionStringOptions ?? throw new ArgumentNullException(nameof(connectionStringOptions));
        }

        public async Task<IEnumerable<TurmaEolDto>> ObterTurmasAlunoHistoricoPorAlunosRa(long[] alunosRa)
        {
            var query = $@"select distinct
							matricula.cd_aluno as alunoRa,
							turesc.cd_turma_escola CodigoTurma,
							turesc.an_letivo AnoLetivo,
							se.sg_resumida_serie as ano_turma,
							case when se.cd_etapa_ensino in (1, 10) or (turesc.cd_tipo_turma <> 1 and e.tp_escola in (10, 11, 12, 14, 15, 18, 26)) or (turesc.cd_tipo_turma <> 1 and e.tp_escola in (2, 17, 28, 30, 31)) then 1 --Infantil
								 when se.cd_etapa_ensino in ( 2, 3, 7, 11 ) then 3 --eja
								 when se.cd_etapa_ensino in ( 4, 5, 12, 13 ) then 5 --fundamental
								 when se.cd_etapa_ensino in ( 6, 7, 8, 17, 14 ) then 6 --médio
							else 0 end as Modalidade,
							matricula.dt_status_matricula DataMatricula,
							matrTurma.dt_situacao_aluno DataSituacao
						from v_historico_matricula_cotic matricula
						inner join v_aluno_cotic aluno on matricula.cd_aluno = aluno.cd_aluno
						inner join historico_matricula_turma_escola matrTurma on matricula.cd_matricula = matrTurma.cd_matricula
						inner join turma_escola turesc on matrTurma.cd_turma_escola = turesc.cd_turma_escola
						inner join serie_turma_escola ste on ste.cd_turma_escola = turesc.cd_turma_escola
						inner join escola e on turesc.cd_escola = e.cd_escola
						inner join serie_ensino se on se.cd_serie_ensino = ste.cd_serie_ensino
						where
							matrTurma.cd_situacao_aluno in (1, 5, 6, 10, 13)
							and matricula.cd_aluno in @alunosRa
						order by matricula.cd_aluno, turesc.an_letivo";

            using var conn = new SqlConnection(connectionStringOptions.Eol);
            return await conn.QueryAsync<TurmaEolDto>(query, new { alunosRa });
        }
    }
}
