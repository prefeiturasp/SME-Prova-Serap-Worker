using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados.Repositorios.Serap
{
    public class RepositorioProvaGrupoPermissao : RepositorioBase<ProvaGrupoPermissao>, IRepositorioProvaGrupoPermissao
    {
        public RepositorioProvaGrupoPermissao(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        public async Task<IEnumerable<ProvaGrupoPermissao>> ObterPorProvaIdAsync(long provaId)
        {
            using var conn = ObterConexao();
            try
            {
                var query = @"select
                                    id 
                                    ,prova_id
                                    ,prova_legado_id 
                                    ,grupo_id 
                                    ,ocultar_prova 
                                    ,criado_em 
                                    ,alterado_em 
                              from prova_grupo_permissao 
                              where prova_id = @provaId";

                return await conn.QueryAsync<ProvaGrupoPermissao>(query, new { provaId });
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
