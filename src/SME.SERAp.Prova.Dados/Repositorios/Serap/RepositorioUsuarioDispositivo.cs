using Dapper;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioUsuarioDispositivo : RepositorioBase<UsuarioDispositivo>, IRepositorioUsuarioDispositivo
    {
        public RepositorioUsuarioDispositivo(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }
    }
}
