using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioExportacaoResultado : RepositorioBase<ExportacaoResultado>, IRepositorioExportacaoResultado
    {
        public RepositorioExportacaoResultado(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }
        public async Task<long> ObterStatusPorIdAsync(long id)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                var query = @"select status from exportacao_resultado where id = @id";
                return await conn.QueryFirstOrDefaultAsync<long>(query, new { id });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
