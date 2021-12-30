using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioResultadoProvaConsolidadoEntity : RepositorioBaseEntity<ResultadoProvaConsolidado>, IRepositorioResultadoProvaConsolidadoEntity
    {
        public RepositorioResultadoProvaConsolidadoEntity(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {

        }
    }
}
