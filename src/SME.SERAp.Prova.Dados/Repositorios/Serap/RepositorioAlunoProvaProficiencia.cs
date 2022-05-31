﻿using SME.SERAp.Prova.Dominio;
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

        public async Task<IEnumerable<AlunoProvaDto>> ObterAlunosSemProficienciaAsync()
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select a.id as alunoId, a.ra as alunoRa, p.id as provaId, p.prova_legado_id as provaLegadoId, p.disciplina_id as disciplinaId, p.ultima_atualizacao as ultimaAtualizacao
                              from prova p
                              left join prova_ano_original pa on pa.prova_id = p.id 
                              left join turma t on t.ano = pa.ano and t.modalidade_codigo = p.modalidade and t.ano_letivo::double precision = date_part('year'::text, p.inicio)
                              join aluno a on a.turma_id = t.id
                              where (p.aderir_todos is null or p.aderir_todos)
	                             and p.formato_tai = true
	                             and not exists(select 1 from aluno_prova_proficiencia ca where ca.prova_id = p.id and ca.aluno_id = a.id and ca.ultima_atualizacao = p.ultima_atualizacao)
		
                              union all 
 
                              select a.id as alunoId, a.ra as alunoRa, p.id as provaId, p.prova_legado_id as provaLegadoId, p.disciplina_id as disciplinaId, p.ultima_atualizacao as ultimaAtualizacao
                              from prova p
                              left join prova_adesao pa on pa.prova_id = p.id
                              join aluno a on a.ra = pa.aluno_ra
                              where p.aderir_todos = false
	                            and p.formato_tai = true
	                            and not exists(select 1 from aluno_prova_proficiencia ca where	ca.prova_id = p.id and ca.aluno_id = a.id  and ca.ultima_atualizacao = p.ultima_atualizacao)";

                return await conn.QueryAsync<AlunoProvaDto>(query);
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
    }
}