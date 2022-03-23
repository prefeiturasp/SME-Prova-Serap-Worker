using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioGrupoSerapCoreSso : RepositorioBase<GrupoSerapCoreSso>, IRepositorioGrupoSerapCoreSso
    {
        public RepositorioGrupoSerapCoreSso(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<IEnumerable<GrupoSerapCoreSso>> ObterGruposSerapCoreSso()
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                const string query = @"select id,
                                                id_coresso IdCoreSso,
                                                nome Nome,
                                                criado_em CriadoEm
                                        from grupo_serap_coresso";
                return await conn.QueryAsync<GrupoSerapCoreSso>(query);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
