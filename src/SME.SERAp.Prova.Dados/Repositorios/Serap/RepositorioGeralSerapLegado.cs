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
    }
}
