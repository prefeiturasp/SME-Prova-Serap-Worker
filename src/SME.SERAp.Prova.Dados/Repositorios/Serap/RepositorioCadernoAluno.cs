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

        public async Task<IEnumerable<ProvaCadernoAlunoDto>> ObterAlunosSemCadernosProvaBibAsync()
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select p.id as provaId, p.total_cadernos as totalCadernos, a.id as alunoId
                              from prova p
                              left join prova_ano_original pa on pa.prova_id = p.id 
                              left join turma t on t.ano = pa.ano and t.modalidade_codigo = p.modalidade and t.ano_letivo::double precision = date_part('year'::text, p.inicio)
                              join aluno a on a.turma_id = t.id
                              where (p.aderir_todos is null or p.aderir_todos)
	                             and p.possui_bib = true
	                             and not exists(select 1 from caderno_aluno ca where ca.prova_id = p.id and aluno_id = a.id)
		
                              union all 
 
                              select p.id as provaId, p.total_cadernos as totalCadernos, a.id as alunoId
                              from prova p
                              left join prova_adesao pa on pa.prova_id = p.id
                              join aluno a on a.ra = pa.aluno_ra
                              where p.aderir_todos = false
	                            and p.possui_bib = true
	                            and not exists(select 1 from caderno_aluno ca where	ca.prova_id = p.id and aluno_id = a.id)";

                return await conn.QueryAsync<ProvaCadernoAlunoDto>(query);
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
    }
}
