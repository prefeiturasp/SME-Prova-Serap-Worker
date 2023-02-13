using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioAlunoProvaProficiencia : RepositorioBase<AlunoProvaProficiencia>, IRepositorioAlunoProvaProficiencia
    {
        public RepositorioAlunoProvaProficiencia(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<bool> ExisteAsync(long alunoId, long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select 1 from aluno_prova_proficiencia where aluno_id = @alunoId and prova_id = @provaId;";
                var existe = await conn.QueryAsync<long>(query, new { alunoId, provaId });
                return existe.Any();
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<AlunoProvaDto>> ObterAlunosSemProficienciaAsync(long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select a.id as alunoId, a.ra as alunoRa, p.id as provaId, p.prova_legado_id as provaLegadoId, p.disciplina_id as disciplinaId, p.ultima_atualizacao as ultimaAtualizacao
                              from prova p
                              left join prova_ano_original pa on pa.prova_id = p.id 
                              left join turma t on (((pa.modalidade = 3 or pa.modalidade = 4) and pa.etapa_eja = t.etapa_eja) or (pa.modalidade <> 3 and pa.modalidade <> 4)) 
  	                                 and t.ano = pa.ano 
  	                                 and t.modalidade_codigo = pa.modalidade 
  	                                 and t.ano_letivo::double precision = date_part('year'::text, p.inicio)  
                              join aluno a on a.turma_id = t.id
                              where (p.aderir_todos is null or p.aderir_todos)
	                             and p.formato_tai = true
	                             and not exists(select 1 from aluno_prova_proficiencia ca where ca.prova_id = p.id and ca.aluno_id = a.id and ca.ultima_atualizacao = p.ultima_atualizacao)
                                 and p.id = @provaId
		
                              union all 
 
                              select a.id as alunoId, a.ra as alunoRa, p.id as provaId, p.prova_legado_id as provaLegadoId, p.disciplina_id as disciplinaId, p.ultima_atualizacao as ultimaAtualizacao
                              from prova p
                              left join prova_adesao pa on pa.prova_id = p.id
                              join aluno a on a.ra = pa.aluno_ra
                              where p.aderir_todos = false
	                            and p.formato_tai = true
	                            and not exists(select 1 from aluno_prova_proficiencia ca where	ca.prova_id = p.id and ca.aluno_id = a.id  and ca.ultima_atualizacao = p.ultima_atualizacao)
                                and p.id = @provaId";

                return await conn.QueryAsync<AlunoProvaDto>(query, new { provaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<decimal> ObterUltimaProficienciaAlunoPorDisciplinaIdAsync(long alunoId, long? disciplinaId)
        {
            using var conn = ObterConexao();
            try
            {
                var tipo = (int)AlunoProvaProficienciaTipo.Final;

                var query = @"select proficiencia  
                              from aluno_prova_proficiencia app
                              left join prova p on p.id = app.prova_id 
                              where app.tipo = @tipo and app.aluno_id = @alunoId and app.proficiencia > 0 ";

                if (disciplinaId.HasValue && disciplinaId.Value > 0)
                    query += " and app.disciplina_id = @disciplinaId";
                else
                    query += " and app.disciplina_id is null";

                query += " order by p.fim desc limit 1";

                return await conn.QueryFirstOrDefaultAsync<decimal>(query, new { alunoId, disciplinaId, tipo });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<decimal> ObterProficienciaInicialAlunoPorProvaIdAsync(long provaId, long alunoId)
        {
            using var conn = ObterConexao();
            try
            {
                var tipo = (int)AlunoProvaProficienciaTipo.Inicial;

                var query = @"select proficiencia  
                              from aluno_prova_proficiencia app
                              where app.tipo = @tipo 
                                and app.aluno_id = @alunoId 
                                and app.proficiencia > 0 
                                and app.prova_id = @provaId";

                return await conn.QueryFirstOrDefaultAsync<decimal>(query, new { provaId, alunoId, tipo });

            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
        public async Task<AlunoProvaProficiencia> ObterProficienciaAlunoAsync(long provaId, long alunoId, AlunoProvaProficienciaTipo tipo, AlunoProvaProficienciaOrigem origem)
        {
            using var conn = ObterConexao();
            try
            {

                var query = @"select id, prova_id as provaId, aluno_id as alunoId, disciplina_id as disciplinaId, 
                                     proficiencia, ra, origem, tipo, ultima_atualizacao as UltimaAtualizacao  
                              from aluno_prova_proficiencia app
                              where app.tipo = @tipo
                                and app.origem = @origem
                                and app.aluno_id = @alunoId 
                                and app.proficiencia > 0 
                                and app.prova_id = @provaId";

                return await conn.QueryFirstOrDefaultAsync<AlunoProvaProficiencia>(query, new { provaId, alunoId, tipo = (int)tipo, origem = (int)origem });

            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<bool> AtualizarValorProficienciaAluno(AlunoProvaProficiencia alunoProvaProficiencia)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"update aluno_prova_proficiencia
                                set proficiencia = @proficiencia,
	                                ultima_atualizacao = @ultimaAtualizacao
                                where id = @id";

                await conn.ExecuteAsync(query, new { 
                                                        id = alunoProvaProficiencia.Id, 
                                                        proficiencia = alunoProvaProficiencia.Proficiencia, 
                                                        ultimaAtualizacao = alunoProvaProficiencia.UltimaAtualizacao 
                                                   });
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
