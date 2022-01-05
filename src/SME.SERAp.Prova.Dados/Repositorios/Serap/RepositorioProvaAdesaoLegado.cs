using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioProvaAdesaoLegado : RepositorioSerapLegadoBase, IRepositorioProvaAdesaoLegado
    {
        public RepositorioProvaAdesaoLegado(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
            
        }

        public async Task<IEnumerable<ProvaAdesao>> ObterAdesaoPorProvaId(long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"
                                select 
	                                t.id ProvaId,
	                                esc.EntityId UeId, 
	                                tur.EntityId TurmaId, 
	                                alu.EntityId AlunoId,
	                                a.alu_matricula AlunoRa,
	                                getdate() CriadoEm,
	                                getdate() AtualizadoEm
                                from Test t
	                                inner join Adherence esc on esc.[State] = 1 and t.id = esc.test_id 
	                                and esc.TypeEntity = 2 and esc.TypeSelection in (1,2)
	                                inner join Adherence tur on tur.[State] = 1 and t.id = tur.test_id 
	                                and tur.TypeEntity = 1 and tur.ParentId = esc.EntityId and tur.TypeSelection in (1,2)
	                                inner join Adherence alu on alu.[State] = 1 and t.id = alu.test_id 
	                                and alu.TypeEntity = 3 and alu.ParentId = tur.EntityId and alu.TypeSelection = 1
	                                inner join SGP_ACA_Aluno a on a.alu_id = alu.EntityId
                                where t.AllAdhered = 0
	                                and t.id = @provaId";

                return await conn.QueryAsync<ProvaAdesao>(query, new { provaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

    }
}
