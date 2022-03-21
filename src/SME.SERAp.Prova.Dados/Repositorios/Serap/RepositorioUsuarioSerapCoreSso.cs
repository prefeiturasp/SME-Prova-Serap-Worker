using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioUsuarioSerapCoreSso : RepositorioBase<UsuarioSerapCoreSso>, IRepositorioUsuarioSerapCoreSso
    {
        public RepositorioUsuarioSerapCoreSso(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<UsuarioSerapCoreSso> ObterPorIdCoreSso(Guid idCoreSso)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                const string query = @"select id,
                                                id_coresso IdCoreSso,
                                                login Login,
                                                nome Nome,
                                                criado_em CriadoEm,
                                                atualizado_em AtualizadoEm
                                        from usuario_serap_coresso
                                        where id_coresso = @idCoreSso";

                return await conn.QueryFirstOrDefaultAsync<UsuarioSerapCoreSso>(query, new { idCoreSso });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public async Task<IEnumerable<UsuarioSerapCoreSso>> ObterPorIdGrupoSerap(long idGrupo)
        {
            using var conn = ObterConexaoLeitura();
            try
            {
                const string query = @"select a.id,
                                                a.id_coresso IdCoreSso,
                                                a.login Login,
                                                a.nome Nome,
                                                a.criado_em CriadoEm,
                                                a.atualizado_em AtualizadoEm
                                        from usuario_serap_coresso a
                                        inner join usuario_grupo_serap_coresso b on a.id = b.id_usuario_serap
                                        where b.id_grupo_serap = @idGrupo";

                return await conn.QueryAsync<UsuarioSerapCoreSso>(query, new { idGrupo });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
