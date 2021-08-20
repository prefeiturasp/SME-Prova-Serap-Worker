using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioAlternativa : RepositorioBase<Alternativas>, IRepositorioAlternativa
    {
        public RepositorioAlternativa(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

    }
}
