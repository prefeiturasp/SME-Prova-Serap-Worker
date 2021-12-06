using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioDreEntity : RepositorioBaseEntity<Dre>, IRepositorioDreEntity
    {
        public RepositorioDreEntity(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {

        }
    }
}
