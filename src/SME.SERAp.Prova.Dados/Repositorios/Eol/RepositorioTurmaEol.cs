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
            var query = $@"select tb.CodigoMatricula as Matricula, 
                                  tb.AlunoRa, 
                                  tb.CodigoTurma, 
                                  tb.AnoLetivo, 
                                  tb.DataMatricula, 
                                  tb.DataSituacao
                           from (
	                            select matricula.CodigoMatricula, 
	                                   matricula.CodigoAluno as AlunoRa, 
		                               matricula.CodigoTurma, 
		                               matricula.AnoLetivo, 
		                               min(matricula.DataSituacao) as DataMatricula, 
		                               max(case when matricula.CodigoSituacaoMatricula <> 1 then matricula.DataSituacao else null end) as DataSituacao
	                            from alunos_matriculas_norm matricula
	                            where matricula.CodigoAluno in @alunosRa  
	                              and matricula.AnoLetivo >= 2021 
                                group by  matricula.CodigoMatricula, matricula.CodigoAluno, matricula.CodigoTurma, matricula.AnoLetivo ) tb
                           order by tb.AnoLetivo, tb.DataMatricula";

            using var conn = new SqlConnection(connectionStringOptions.Eol);
            return await conn.QueryAsync<TurmaEolDto>(query, new { alunosRa });
        }
    }
}
