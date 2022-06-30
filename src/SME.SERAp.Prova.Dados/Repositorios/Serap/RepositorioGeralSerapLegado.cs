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
    public class RepositorioGeralSerapLegado : RepositorioSerapLegadoBase, IRepositorioGeralSerapLegado
    {
        public RepositorioGeralSerapLegado(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {

        }

        public async Task<TipoProva> ObterTipoProvaLegadoPorId(long tipoProvaLegadoId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select
                                    Id as LegadoId,
                                    [Description] as Descricao, 
                                    TargetToStudentsWithDeficiencies as ParaEstudanteComDeficiencia 
                                from TestType 
                                    where [State] = 1 and Id = @tipoProvaLegadoId";


                return await conn.QueryFirstOrDefaultAsync<TipoProva>(query, new { tipoProvaLegadoId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<Guid>> ObterTipoProvaDeficienciaPorTipoProvaLegadoId(long tipoProvaLegadoId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select DeficiencyId 
                                from TestTypeDeficiency 
                                    where [State] = 1
                                    and TestType_Id = @tipoProvaLegadoId";

                return await conn.QueryAsync<Guid>(query, new { tipoProvaLegadoId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<AreaConhecimentoSerap> ObterAreaConhecimentoSerapPorDisciplinaId(long disciplinaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"   select distinct a.Id AreaConhecimentoId
								   from KnowledgeArea a
							 inner join KnowledgeAreaDiscipline ad
									 on a.id = ad.KnowledgeArea_Id
							 inner join Discipline d
									 on d.Id = ad.Discipline_Id
								  where a.[State] = 1
									and ad.[State] = 1
									and d.[State] = 1
									and ad.Discipline_Id is not null
									and d.Id = @disciplinaId";

                return await conn.QueryFirstOrDefaultAsync<AreaConhecimentoSerap>(query, new { disciplinaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
