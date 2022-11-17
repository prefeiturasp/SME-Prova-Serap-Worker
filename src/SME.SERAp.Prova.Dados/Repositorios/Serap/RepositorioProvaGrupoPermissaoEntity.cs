using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;

namespace SME.SERAp.Prova.Dados.Repositorios.Serap
{
    public class RepositorioProvaGrupoPermissaoEntity : RepositorioBaseEntity<ProvaGrupoPermissao>, IRepositorioProvaGrupoPermissaoEntity
    {
        public RepositorioProvaGrupoPermissaoEntity(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {
        }
    }
}