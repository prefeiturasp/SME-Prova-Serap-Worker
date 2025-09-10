using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
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

        public async Task<IEnumerable<AlunoProvaDto>> ObterAlunosSemProficienciaPorProvaIdAsync(long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                const string query = @"select p.id as provaId,
                                            p.prova_legado_id as provaLegadoId,
                                            p.disciplina_id as disciplinaId,
                                            p.ultima_atualizacao as ultimaAtualizacao,
                                            a.id as alunoId,
                                            a.ra as alunoRa,
                                            t.ano,
                                            t.ano_letivo as anoletivo,
                                            t.id as turmaid,
                                            t.ue_id as ueid,
                                            u.dre_id as dreid,
                                            u.ue_id as uecodigo
                                        from prova p
                                        left join prova_ano_original pa on pa.prova_id = p.id
                                        left join turma t on (((pa.modalidade = 3 or pa.modalidade = 4) and pa.etapa_eja = t.etapa_eja) or (pa.modalidade <> 3 and pa.modalidade <> 4))
  	                                        and t.ano = pa.ano 
  	                                        and t.modalidade_codigo = pa.modalidade
  	                                        and t.ano_letivo::double precision = date_part('year'::text, p.inicio)
                                        join aluno a on a.turma_id = t.id
                                        join ue u on u.id = t.ue_id
                                        where (p.aderir_todos is null or p.aderir_todos)
	                                    and p.formato_tai = true
	                                    and not exists(select 1 
                                                        from aluno_prova_proficiencia ca 
                                                        where ca.prova_id = p.id 
                                                        and ca.aluno_id = a.id 
                                                        and ca.ultima_atualizacao = p.ultima_atualizacao)
                                        and p.id = @provaId
		
                                        union all
 
                                        select p.id as provaId,
                                            p.prova_legado_id as provaLegadoId,
                                            p.disciplina_id as disciplinaId,
                                            p.ultima_atualizacao as ultimaAtualizacao,
                                            a.id as alunoId,
                                            a.ra as alunoRa,
                                            t.ano,
                                            t.ano_letivo as anoletivo,
                                            t.id as turmaid,
                                            t.ue_id as ueid,
                                            u.dre_id as dreid,
                                            u.ue_id as uecodigo
                                        from prova p
                                        join prova_adesao pa on pa.prova_id = p.id
                                        join aluno a on a.ra = pa.aluno_ra
                                        join prova_ano_original pao on pao.prova_id = p.id
                                        join turma t on t.id = a.turma_id
                                            and t.ano = pao.ano
                                        join ue u on u.id = t.ue_id
                                        where p.aderir_todos = false
	                                    and p.formato_tai = true
	                                    and not exists(select 1
	                                                    from aluno_prova_proficiencia ca
	                                                    where ca.prova_id = p.id 
	                                                    and ca.aluno_id = a.id 
	                                                    and ca.ultima_atualizacao = p.ultima_atualizacao)
                                        and p.id = @provaId";

                return await conn.QueryAsync<AlunoProvaDto>(query, new { provaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<bool> ExisteAsync(long alunoId, long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                const string query = @"select 1 from aluno_prova_proficiencia where aluno_id = @alunoId and prova_id = @provaId;";
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
                const string query = @"select p.id as provaId,
                                            p.prova_legado_id as provaLegadoId,
                                            p.disciplina_id as disciplinaId,
                                            p.ultima_atualizacao as ultimaAtualizacao,
                                            a.id as alunoId,
                                            a.ra as alunoRa,
                                            t.ano,
                                            t.ano_letivo as anoletivo,
                                            t.id as turmaid,
                                            t.ue_id as ueid,
                                            u.dre_id as dreid,
                                            u.ue_id as uecodigo
                                        from prova p
                                        left join prova_ano_original pa on pa.prova_id = p.id
                                        left join turma t on (((pa.modalidade = 3 or pa.modalidade = 4) and pa.etapa_eja = t.etapa_eja) or (pa.modalidade <> 3 and pa.modalidade <> 4))
  	                                        and t.ano = pa.ano 
  	                                        and t.modalidade_codigo = pa.modalidade 
  	                                        and t.ano_letivo::double precision = date_part('year'::text, p.inicio)
                                        join aluno a on a.turma_id = t.id
                                        join ue u on u.id = t.ue_id
                                        where p.formato_tai = true
	                                    and (p.aderir_todos or p.aderir_todos is null)
	                                    and not exists(select 1 
                                                        from aluno_prova_proficiencia app
                                                        where app.prova_id = p.id
                                                        and app.aluno_id = a.id
                                                        and app.ultima_atualizacao = p.ultima_atualizacao)
		
                                        union all 
 
                                        select p.id as provaid,
                                            p.prova_legado_id as provalegadoid,
                                            p.disciplina_id as disciplinaid,
                                            p.ultima_atualizacao as ultimaatualizacao,
                                            a.id as alunoid,
                                            a.ra as alunora,
                                            t.ano,
                                            t.ano_letivo as anoletivo,
                                            t.id as turmaid,
                                            t.ue_id as ueid,
                                            u.dre_id as dreid,
                                            u.ue_id as uecodigo
                                        from prova p
                                        join prova_adesao pa on pa.prova_id = p.id
                                        join aluno a on a.ra = pa.aluno_ra
                                        join prova_ano_original pao on pao.prova_id = p.id
                                        join turma t on t.id = a.turma_id
	                                        and t.ano = pao.ano
                                        join ue u on u.id = t.ue_id
                                        where p.formato_tai = true
	                                    and p.aderir_todos = false
                                        and not exists (select 1
				                                        from aluno_prova_proficiencia app
				                                        where app.prova_id = p.id
				                                        and app.aluno_id = a.id
				                                        and app.ultima_atualizacao = p.ultima_atualizacao)";

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
                const int tipo = (int)AlunoProvaProficienciaTipo.Final;

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
                const int tipo = (int)AlunoProvaProficienciaTipo.Inicial;

                const string query = @"select proficiencia  
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
                const string query = @"select id,
                                            prova_id as provaId,
                                            aluno_id as alunoId,
                                            disciplina_id as disciplinaId,
                                            proficiencia,
                                            ra,
                                            origem,
                                            tipo,
                                            ultima_atualizacao as UltimaAtualizacao
                                        from aluno_prova_proficiencia app
                                        where app.tipo = @tipo
                                        and app.origem = @origem
                                        and app.aluno_id = @alunoId
                                        and app.proficiencia > 0
                                        and app.prova_id = @provaId";

                return await conn.QueryFirstOrDefaultAsync<AlunoProvaProficiencia>(query,
                    new { provaId, alunoId, tipo = (int)tipo, origem = (int)origem });
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
                const string query = @"update aluno_prova_proficiencia
                                        set proficiencia = @proficiencia,
	                                        ultima_atualizacao = @ultimaAtualizacao,
	                                        erro_medida = @erroMedida
                                        where id = @id";

                await conn.ExecuteAsync(query, new
                {
                    id = alunoProvaProficiencia.Id,
                    proficiencia = alunoProvaProficiencia.Proficiencia,
                    ultimaAtualizacao = alunoProvaProficiencia.UltimaAtualizacao,
                    erroMedida = alunoProvaProficiencia.ErroMedida
                });
                return true;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<int> ExcluirAlunoProvaProficiencia(long provaId, long alunoRa)
        {
            using var conn = ObterConexao();
            try
            {
                var query = $@"delete
                                from
	                                aluno_prova_proficiencia
                                where
	                                id in (
	                                select
		                                app.id
	                                from
		                                aluno_prova_proficiencia app
	                                inner join aluno a on
		                                a.id = app.aluno_id
	                                where
		                                a.ra = @alunoRa
		                                and app.prova_id = @provaId
		                                and app.tipo <> 0)";

                return await conn.ExecuteAsync(query, new { provaId, alunoRa });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<AlunoProvaProficienciaBoletimDto>> ObterAlunosProvaProficienciaBoletimPorProvaId(long provaId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"
                    WITH NivelAluno AS (
                        SELECT 
                            np.disciplina_id, 
                            np.ano, 
                            np.codigo AS NivelCodigo, 
                            np.valor_referencia 
                        FROM nivel_proficiencia np
                    )
                    SELECT
                        u.dre_id as CodigoDre,
                        u.ue_id as CodigoUe,
                        u.nome as NomeUe,
                        p.id as ProvaId,
                        p.descricao as NomeProva,
                        NULLIF(REGEXP_REPLACE(t.ano, '[^0-9]', '', 'g'), '')::INTEGER as AnoEscolar,
                        t.nome as Turma,
                        a.ra as CodigoAluno,
                        a.nome as NomeAluno,
                        p.disciplina as NomeDisciplina,
                        p.disciplina_id as DisciplinaId,
                        app.tipo as ProvaStatus,
                        app.proficiencia as Proficiencia,
                        app.erro_medida as ErroMedida,
                        COALESCE(
                            (SELECT np.NivelCodigo 
                             FROM NivelAluno np
                             WHERE np.disciplina_id = p.disciplina_id 
                               AND np.ano = NULLIF(REGEXP_REPLACE(t.ano, '[^0-9]', '', 'g'), '')::INTEGER
                               AND (app.proficiencia < np.valor_referencia OR np.valor_referencia IS NULL)
                             ORDER BY np.NivelCodigo ASC
                             LIMIT 1), 
                            4) AS NivelCodigo
                    FROM 
                        aluno_prova_proficiencia app
                    INNER JOIN prova p ON 
                        p.id = app.prova_id and 
                        p.exibir_no_boletim 
                    INNER JOIN aluno a ON 
                        a.id = app.aluno_id
                    INNER JOIN turma t ON 
                        t.id = a.turma_id
                    INNER JOIN ue u ON 
                        u.id = t.ue_id
                    WHERE 
                        app.prova_id = @provaId AND 
                        app.tipo IN ('2') AND 
                        NULLIF(REGEXP_REPLACE(t.ano, '[^0-9]', '', 'g'), '')::INTEGER >= 5;";

                return await conn.QueryAsync<AlunoProvaProficienciaBoletimDto>(query, new { provaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}