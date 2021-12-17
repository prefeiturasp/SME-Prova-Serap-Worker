using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioProvaAluno : RepositorioBase<Dominio.ProvaAluno>, IRepositorioProvaAluno
    {
        public RepositorioProvaAluno(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<ProvaAluno> ObterPorProvaIdRaAsync(long provaId, long alunoRa)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select distinct pa.* from prova_aluno pa where pa.prova_id = @provaId and pa.aluno_ra = @alunoRa";

                return await conn.QueryFirstOrDefaultAsync<ProvaAluno>(query, new { provaId, alunoRa });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
        public async Task<IEnumerable<ProvaAluno>> ObterPorProvaIdsRaAsync(long[] provaIds, long alunoRa)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select distinct pa.* from v_provas_alunos pa where pa.prova_id = ANY(@provaIds) and pa.aluno_ra = @alunoRa";

                return await conn.QueryAsync<ProvaAluno>(query, new { provaIds, alunoRa });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
        public async Task<ProvaAluno> ObterPorQuestaoIdRaAsync(long questaoId, long alunoRa)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select distinct pa.* from prova_aluno pa 
                                inner join questao q on pa.prova_id = q.prova_id 
                                where q.id = @questaoId and pa.aluno_ra = @alunoRa";

                return await conn.QueryFirstOrDefaultAsync<ProvaAluno>(query, new { questaoId, alunoRa });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<ProvaAluno> ObterPorProvaIdRaStatusAsync(long provaId, long alunoRa, int status)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select distinct pa.* from prova_aluno pa where pa.prova_id = @provaId and pa.aluno_ra = @alunoRa and pa.status = @status";

                return await conn.QueryFirstOrDefaultAsync<ProvaAluno>(query, new { provaId, alunoRa, status });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<ProvaAlunoReduzidaDto>> ObterAlunosProvasFinalizadasReduzido()
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select distinct id as ProvaAlunoId, aluno_ra as AlunoRa, finalizado_em::date as FinalizadoEm 
                    from prova_aluno pa 
                    where finalizado_em is not null and (frequencia is null or frequencia != 0) and status = 2 limit 10;";

                return await conn.QueryAsync<ProvaAlunoReduzidaDto>(query);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task AtualizarFrequenciaAlunoAsync(long id, FrequenciaAluno frequencia)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"update prova_aluno set frequencia = @frequencia where id = @id;";

                await conn.ExecuteAsync(query, new { frequencia, id });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
