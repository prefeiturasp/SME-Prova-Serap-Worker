using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioTipoProvaDeficiencia : RepositorioBase<TipoProvaDeficiencia>, IRepositorioTipoProvaDeficiencia
    {
        public RepositorioTipoProvaDeficiencia(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<bool> RemoverPorTipoProvaId(long tipoProvaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"delete from tipo_prova_deficiencia where tipo_prova_id = @tipoProvaId;";

                await conn.QueryAsync(query, new { tipoProvaId });
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
