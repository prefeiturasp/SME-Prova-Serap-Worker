using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioTipoProva : RepositorioBase<TipoProva>, IRepositorioTipoProva
    {
        public RepositorioTipoProva(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<TipoProva> ObterPorLegadoId(long legadoId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select 
                                id, 
                                legado_id LegadoId, 
                                descricao Descricao, 
                                para_estudante_com_deficiencia ParaEstudanteComDeficiencia, 
                                criado_em CriadoEm, 
                                atualizado_em AtualizadoEm
                                from tipo_prova 
                                where legado_id = @legadoId;";

                return await conn.QueryFirstOrDefaultAsync<TipoProva>(query, new { legadoId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
