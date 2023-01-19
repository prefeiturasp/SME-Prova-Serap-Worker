using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioTurmaAlunoHistorico : RepositorioBase<TurmaAlunoHistorico>, IRepositorioTurmaAlunoHistorico
    {
        public RepositorioTurmaAlunoHistorico(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<IEnumerable<TurmaAlunoHistoricoDto>> ObterTurmaAlunoHistoricoPorAlunosRa(long[] alunosRa)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select a.ra as alunoRa, 
                                     tah.id, 
                                     tah.turma_id as turmaId, 
                                     tah.ano_letivo as anoLetivo, 
                                     a.id as alunoId, 
                                     tah.data_matricula as dataMatricula, 
                                     tah.data_situacao as dataSituacao,
                                     tah.matricula               
                            from aluno a
                            left join turma_aluno_historico tah ON tah.aluno_id = a.id 
                            where a.ra = ANY(@alunosRa)";
                return await conn.QueryAsync<TurmaAlunoHistoricoDto>(query, new { alunosRa });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
