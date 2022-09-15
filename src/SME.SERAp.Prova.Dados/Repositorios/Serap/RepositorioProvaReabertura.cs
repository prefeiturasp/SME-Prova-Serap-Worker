using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;


namespace SME.SERAp.Prova
{
   public class RepositorioProvaReabertura : RepositorioBase<ProvaAlunoReabertura>, IRepositorioProvaAlunoReabertura
    {
        public RepositorioProvaReabertura(ConnectionStringOptions connectionStringOptions) : base(connectionStringOptions)
        {

        }

        
    }
}
