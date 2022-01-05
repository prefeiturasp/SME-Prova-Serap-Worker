using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioProvaAdesao : RepositorioBase<ProvaAdesao>, IRepositorioProvaAdesao
    {
        public RepositorioProvaAdesao(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<IEnumerable<ProvaAdesao>> ObterDadosAlunosParaAdesaoPorRa(long[] alunosRa)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"
                                select
                                    a.id AlunoId,
                                    t.id TurmaId,
                                    ue.id UeId
                                from aluno a
                                    inner join turma t on a.turma_id = t.id
                                    inner join ue ue on ue.id = t.ue_id
                                where a.ra = any(@alunosRa)";

                return await conn.QueryAsync<ProvaAdesao>(query, new { alunosRa });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task ExcluirAdesaoPorProvaId(long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = $@"delete from prova_adesao where prova_id = @provaId;";
                await conn.ExecuteAsync(query, new { provaId });
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

    }
}
