using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System.Threading.Tasks;
using Dapper;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioUsuarioGrupoSerapCoreSso : RepositorioBase<UsuarioGrupoSerapCoreSso>, IRepositorioUsuarioGrupoSerapCoreSso
    {
        public RepositorioUsuarioGrupoSerapCoreSso(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<UsuarioGrupoSerapCoreSso> ObterPorUsuarioIdEGrupoIdCoreSso(long usuarioId, long grupoId)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                const string query = @"select id, id_usuario_serap IdUsuarioSerapCoreSso, id_grupo_serap IdGrupoSerapCoreSso, criado_em CriadoEm
                                        from usuario_grupo_serap_coresso
                                        where id_usuario_serap = @usuarioId
                                            and id_grupo_serap = @grupoId;";

                return await conn.QueryFirstOrDefaultAsync<UsuarioGrupoSerapCoreSso>(query, new { usuarioId, grupoId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<bool> RemoverPorUsuarioSerapIdEGrupoId(long usuarioSerapId, long grupoSerapId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"delete
                                        from usuario_grupo_serap_coresso
                                        where id_usuario_serap = @usuarioSerapId
                                            and id_grupo_serap = @grupoSerapId;";

                await conn.QueryAsync(query, new { usuarioSerapId, grupoSerapId });
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
