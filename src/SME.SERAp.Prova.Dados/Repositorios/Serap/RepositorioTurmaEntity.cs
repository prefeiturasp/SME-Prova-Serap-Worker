using Microsoft.EntityFrameworkCore;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioTurmaEntity : RepositorioBaseEntity<Turma>, IRepositorioTurmaEntity
    {
        public RepositorioTurmaEntity(DbContextOptions<ContextoDbSerap> dbContextOptions) : base(dbContextOptions)
        {

        }
    }
}
