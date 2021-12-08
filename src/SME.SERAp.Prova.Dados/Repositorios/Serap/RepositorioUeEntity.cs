using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioUeEntity : RepositorioBaseEntity<Ue>, IRepositorioUeEntity
    {
        public RepositorioUeEntity(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {

        }
    }
}
