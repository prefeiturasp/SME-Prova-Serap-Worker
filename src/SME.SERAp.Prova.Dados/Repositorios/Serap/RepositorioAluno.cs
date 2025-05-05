using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioAluno : RepositorioBase<Aluno>, IRepositorioAluno
    {
        public RepositorioAluno(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<long> InserirOuAtualizarAlunoAsync(AlunoEolDto aluno)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"WITH upsert AS (
                            UPDATE aluno
                            SET
                                nome = @nome, 
		                          turma_id = @turmaId, 
		                          situacao = @situacao,
                                data_atualizacao = @dataAtualizacao
                            WHERE
                                ra = @ra
                            RETURNING *
                        )
                        INSERT INTO aluno(nome, ra, turma_id, situacao,data_atualizacao)
                        SELECT
                            @nome,  @ra, @turmaId, @situacao,@dataAtualizacao
                        WHERE
                            NOT EXISTS (SELECT 1 FROM upsert);
                        select id from aluno where ra = @ra;";

                return await conn.QueryFirstOrDefaultAsync<long>(query, new
                {
                    nome = aluno.Nome,
                    turmaId = aluno.TurmaSerapId,
                    ra = aluno.CodigoAluno,
                    situacao = aluno.SituacaoAluno,
                    dataAtualizacao = DateTime.Now
                });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<Aluno> ObterAlunoPorCodigo(long codigo)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select id, nome, turma_id as TurmaId, ra from aluno where ra = @codigo";

                return await conn.QueryFirstOrDefaultAsync<Aluno>(query, new { codigo });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }


        public async Task<IEnumerable<AlunoCadernoProvaTaiTratarDto2>> Correcao()
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select c.prova_id as provaid, c.aluno_id as alunoid, c.caderno as caderno, a.aluno_ra as alunora  
from caderno_aluno c 
inner join aluno al on (al.id = c.aluno_id) 
inner join prova_aluno a on (a.prova_id = c.prova_id and a.aluno_ra = al.ra)
where c.prova_id in (548, 549, 550, 551) and a.status = 1 and a.criado_em >= '2025-04-23'


and (al.ra = 5219029 
or al.ra = 8192209 
or al.ra = 6081513 
or al.ra = 5655740 
or al.ra = 5560801 
or al.ra = 5665155 

or al.ra = 6111668 
or al.ra = 5604730 
or al.ra = 8400610 
or al.ra = 6215436 
or al.ra = 7831951 
or al.ra = 1330259 
or al.ra = 5193247 
or al.ra = 6278456 
or al.ra = 5127989 
or al.ra = 5564993 

or al.ra = 5640411 
or al.ra = 6228106 
or al.ra = 6034116 
or al.ra = 5602133 
or al.ra = 5226914 
or al.ra = 6144696 
or al.ra = 5626142 
or al.ra = 4764812






or al.ra = 6233933 
or al.ra = 5788737 
or al.ra = 5964424 
or al.ra = 5754917 
or al.ra = 5189563
or al.ra = 5127393
or al.ra = 6497545








)








";

                return await conn.QueryAsync<AlunoCadernoProvaTaiTratarDto2>(query);
            }
            catch ( Exception e)
            {

                var obj = e;
                return default;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<Aluno>> ObterAlunoPorCodigosAsync(long[] codigos)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select id, nome, turma_id as TurmaId, ra, situacao, data_atualizacao, nome_social, data_nascimento, sexo from aluno where ra = ANY(@codigos)";

                return await conn.QueryAsync<Aluno>(query, new { codigos });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<Aluno>> ObterAlunosPorTurmaIdAsync(long turmaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select id, nome, ra, turma_id as turmaId, situacao, data_atualizacao as DataAtualizacao, nome_social as nomeSocial, data_nascimento as dataNascimento, sexo from aluno where turma_id = @turmaId";

                return await conn.QueryAsync<Aluno>(query, new { turmaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<Aluno>> ObterAlunosPorTurmasIdsAsync(long[] turmasIds)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                const string query = @"select id, 
                                            nome, 
                                            turma_id as TurmaId, 
                                            ra, 
                                            Situacao, 
                                            data_atualizacao, 
                                            nome_social, 
                                            data_nascimento, 
                                            sexo 
                                        from aluno 
                                        where turma_id = ANY(@turmasIds)";

                return await conn.QueryAsync<Aluno>(query, new { turmasIds });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<Aluno>> ObterTodosAsync()
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select id, nome, turma_id as TurmaId, ra, Situacao from aluno ";

                return await conn.QueryAsync<Aluno>(query);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<Aluno>> ObterAlunosAdesaoPorProvaId(long provaId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select a.id, a.nome, a.turma_id as TurmaId, a.ra, a.Situacao 
                                from prova_adesao pa
                                inner join aluno a on pa.aluno_ra = a.ra
                                where pa.prova_id = @provaId";

                return await conn.QueryAsync<Aluno>(query, new { provaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<ProvaAlunoTaiSemCadernoDto>> ObterAlunosProvaTaiSemCadernoProvaId(long provaId, string ano)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select f.prova_id as ProvaId,
                                     f.aluno_id as AlunoId,
                                     f.aluno_situacao as Situacao,
                                     f.prova_legado_id as ProvaLegadoId,
                                     f.aluno_ra as AlunoRa
                                from v_prova_turma_aluno f
                               where f.formato_tai = true
                                 and f.prova_id = @provaId
                                 and f.turma_ano = @ano
                                 and not exists(select 1
                                                  from caderno_aluno ca 
                                                  where	ca.prova_id = f.prova_id 
                                                    and ca.aluno_id = f.aluno_id)
                                order by f.prova_id";

                return await conn.QueryAsync<ProvaAlunoTaiSemCadernoDto>(query, new { provaId, ano });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
        
        public async Task<IEnumerable<ProvaAlunoTaiSemCadernoDto>> ObterAlunosProvaTaiSemCaderno(string ano)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select f.prova_id as ProvaId,
                                     f.aluno_id as AlunoId,
                                     f.aluno_situacao as Situacao,
                                     f.prova_legado_id as ProvaLegadoId,
                                     a.ra as AlunoRa
                                from v_prova_turma_aluno f
                                inner join aluno a on a.id = f.aluno_id
                               where f.formato_tai = true
                                 and f.turma_ano = @ano
                                 and not exists(select 1
                                                  from caderno_aluno ca 
                                                  where	ca.prova_id = f.prova_id 
                                                    and ca.aluno_id = f.aluno_id)
                                order by f.prova_id";

                return await conn.QueryAsync<ProvaAlunoTaiSemCadernoDto>(query, new { ano });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<Aluno> ObterAlunoPorIdAsync(long alunoId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select top 1 id, nome, turma_id as TurmaId, ra, Situacao from aluno where id = @alunoId";

                return await conn.QueryFirstOrDefaultAsync<Aluno>(query, new { alunoId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<bool> InativarAlunoPorIdETurmaIdAsync(long turmaId, long[] alunosId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"update aluno set situacao = 99 where turma_id = @turmaId and id = ANY(@alunosId);";
                return await conn.ExecuteAsync(query, new { turmaId, alunosId }) > 1;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

        }
    }
}

