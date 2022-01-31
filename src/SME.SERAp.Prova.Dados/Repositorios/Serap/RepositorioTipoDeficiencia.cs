using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioTipoDeficiencia : RepositorioBase<TipoDeficiencia>, IRepositorioTipoDeficiencia
    {
        public RepositorioTipoDeficiencia(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<TipoDeficiencia> ObterPorLegadoId(Guid legadoId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select 
                                id Id,
	                            legado_id LegadoId,
	                            codigo_eol CodigoEol,
	                            nome Nome,
	                            criado_em CriadoEm,
	                            atualizado_em AtualizadoEm 
                              from tipo_deficiencia
                                where legado_id = @legadoId;";

                return await conn.QueryFirstOrDefaultAsync<TipoDeficiencia>(query, new { legadoId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<TipoDeficiencia> ObterPorCodigoEol(int codigoEol)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select 
	                            id Id,
	                            legado_id LegadoId,
	                            codigo_eol CodigoEol,
	                            nome Nome,
	                            criado_em CriadoEm,
	                            atualizado_em AtualizadoEm 
                             from tipo_deficiencia
                                where codigo_eol = @codigoEol;";

                return await conn.QueryFirstOrDefaultAsync<TipoDeficiencia>(query, new { codigoEol });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
