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
                var query = @"";

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
