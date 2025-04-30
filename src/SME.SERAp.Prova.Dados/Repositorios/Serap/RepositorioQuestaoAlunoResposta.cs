using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.Dtos;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioQuestaoAlunoResposta : RepositorioBase<QuestaoAlunoResposta>, IRepositorioQuestaoAlunoResposta
    {
        public RepositorioQuestaoAlunoResposta(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<QuestaoAlunoResposta> ObterPorIdRaAsync(long questaoId, long alunoRa)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select * from questao_aluno_resposta 
                        where questao_id = @questaoId and 
                        aluno_ra = @alunoRa";

                return await conn.QueryFirstOrDefaultAsync<QuestaoAlunoResposta>(query, new { questaoId, alunoRa });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<AlunoProvaRespostaSemPerguntaDto>> ObterAlunoComRespostasSemQuestoes(DateTime inicio, DateTime fim, long? alunoRa)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select a.id as AlunoId, a.ra as AlunoRa, q.prova_id as ProvaId from questao_aluno_resposta qar
                            inner join aluno a on a.ra = qar.aluno_ra
                            inner join questao q on q.id = qar.questao_id
                            inner join prova_aluno pa on pa.aluno_ra = a.ra and pa.prova_id = q.prova_id
                            inner join prova p on p.id = q.prova_id
                            where qar.criado_em between @inicio and @fim
                            and not exists(select 1 from questao_aluno_tai qat where qat.questao_id = qar.questao_id and qat.aluno_id = a.id)
                            and pa.status = 1
                            and p.formato_tai";

                if(alunoRa > 0)
                {
                    query += " and a.ra = @alunoRa";
                }

                query += @" group by a.id, a.ra, q.prova_id
                            order by a.id, a.ra, q.prova_id";

                return await conn.QueryAsync<AlunoProvaRespostaSemPerguntaDto>(query, new { inicio, fim, alunoRa });
            }
            catch(Exception ex)
            {
                return default;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<QuestaoAlunoRespostaComOrdemDto>> ObterQuestoesAlunoRespostaComOrdem(long alunoRa, long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select (row_number() over (order by criado_em) - 1) as Ordem, 
	                            qar.id as id,
	                            qar.questao_id as QuestaoId,
	                            qar.aluno_ra as AlunoRa, 
	                            qar.alternativa_id as AlternativaId,
	                            qar.criado_em as CriadoEm,
	                            qar.dispositivo_id as DispositivoId
                            from questao_aluno_resposta qar
                            inner join questao q on q.id = qar.questao_id
                            where qar.aluno_ra = @alunoRa and q.prova_id = @provaId";

                return await conn.QueryAsync<QuestaoAlunoRespostaComOrdemDto>(query, new { alunoRa, provaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
