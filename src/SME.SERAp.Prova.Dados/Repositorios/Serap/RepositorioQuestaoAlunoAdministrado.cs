using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioQuestaoAlunoAdministrado : RepositorioBase<QuestaoAlunoAdministrado>, IRepositorioQuestaoAlunoAdministrado
    {
        public RepositorioQuestaoAlunoAdministrado(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }
    }
}