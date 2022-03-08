using Dapper;
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

        public async Task<IEnumerable<TurmaEolDto>> ObterTurmasAlunoHistoricoPorAlunoRa(long alunoRa)
        {
            var query = $@"select 
                                turesc.cd_turma_escola CodigoTurma, 
                                turesc.an_letivo AnoLetivo,
                                MAX(matrTurma.dt_situacao_aluno) dt_situacao_aluno
                            FROM v_historico_matricula_cotic matricula
                                    INNER JOIN v_aluno_cotic aluno ON
	                            matricula.cd_aluno = aluno.cd_aluno
                                    INNER JOIN historico_matricula_turma_escola matrTurma ON
	                            matricula.cd_matricula = matrTurma.cd_matricula
                                    INNER JOIN turma_escola turesc ON
	                            matrTurma.cd_turma_escola = turesc.cd_turma_escola
                            where matricula.cd_aluno = @alunoRa
                            group by turesc.cd_turma_escola, turesc.an_letivo
                            order by turesc.an_letivo";

            using var conn = new SqlConnection(connectionStringOptions.Eol);
            return await conn.QueryAsync<TurmaEolDto>(query, new { alunoRa });
        }

    }
}
