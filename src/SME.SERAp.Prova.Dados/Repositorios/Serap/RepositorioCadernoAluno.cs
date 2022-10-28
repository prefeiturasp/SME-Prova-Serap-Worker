using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioCadernoAluno : RepositorioBase<CadernoAluno>, IRepositorioCadernoAluno
    {
        public RepositorioCadernoAluno(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<IEnumerable<ProvaCadernoAlunoDto>> ObterAlunosSemCadernosProvaBibAsync(long provaId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"SELECT vpta.prova_id as provaId, vpta.turma_ano as ano, vpta.aluno_id as alunoId
                              FROM v_prova_turma_aluno vpta
                              WHERE vpta.prova_id = @provaId
                                and not exists(select 1 from caderno_aluno ca where ca.prova_id = vpta.prova_id and ca.aluno_id = vpta.aluno_id)";

                return await conn.QueryAsync<ProvaCadernoAlunoDto>(query, new { provaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<bool> RemoverCadernosPorProvaIdAsync(long provaId)
        {

            using var conn = ObterConexao();
            try
            {
                var query = @"delete  from caderno_aluno where prova_id = @provaId";

                await conn.ExecuteAsync(query, new { provaId });

                return true;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<CadernoAluno> ObterCadernoAlunoPorProvaIdAlunoIdAsync(long provaId, long alunoId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select id, prova_id, aluno_id, caderno
                              from caderno_aluno 
                              where prova_id = @provaId and aluno_id = @alunoId";

                return await conn.QueryFirstOrDefaultAsync<CadernoAluno>(query, new { provaId, alunoId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
