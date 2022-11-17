using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.Dtos;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioProvaAno : RepositorioBase<ProvaAno>, IRepositorioProvaAno
    {
        public RepositorioProvaAno(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<bool> RemoverAnosPorProvaIdAsync(long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"delete from prova_ano where prova_id = @provaId";

                await conn.ExecuteAsync(query, new { provaId });

                return true;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<TipoCurriculoPeriodoAnoDto>> ObterProvaAnoPorTipoCurriculoPeriodoId(int[] tcpIds)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select tcp_id as tcpId, ano, modalidade_codigo as Modalidade,etapa_eja as EtapaEja
                              from tipo_curriculo_periodo_ano tcpa 
                              where tcpa.tcp_id = ANY(@tcpIds)";

                return await conn.QueryAsync<TipoCurriculoPeriodoAnoDto>(query, new { tcpIds });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
