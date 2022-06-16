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

        public async Task<IEnumerable<Aluno>> ObterAlunoPorCodigosAsync(long[] codigos)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select id, nome, turma_id as TurmaId, ra from aluno where ra = ANY(@codigos)";

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
            using var conn = ObterConexaoLeitura();
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

        public async Task<IEnumerable<Aluno>> ObterAlunosPorTurmasCodigoAsync(long[] turmasCodigo) 
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select id, nome, turma_id as TurmaId, ra, Situacao from aluno where turma_id = ANY(@turmasCodigo)";

                return await conn.QueryAsync<Aluno>(query, new { turmasCodigo });
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
    }
}
