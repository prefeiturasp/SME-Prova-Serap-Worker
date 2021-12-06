using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioAlunoEntity : RepositorioBaseEntity<Aluno>, IRepositorioAlunoEntity
    {
        public RepositorioAlunoEntity(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {

        }
    }
}
