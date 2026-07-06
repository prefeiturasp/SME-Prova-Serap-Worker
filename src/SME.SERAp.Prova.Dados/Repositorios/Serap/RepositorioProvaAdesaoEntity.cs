using Microsoft.EntityFrameworkCore;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioProvaAdesaoEntity : RepositorioBaseEntity<ProvaAdesao>, IRepositorioProvaAdesaoEntity
    {
        public RepositorioProvaAdesaoEntity(DbContextOptions<ContextoDbSerap> dbContextOptions) : base(dbContextOptions)
        {

        }
    }
}
