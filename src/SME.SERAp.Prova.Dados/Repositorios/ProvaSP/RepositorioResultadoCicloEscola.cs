using System.Threading.Tasks;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.EnvironmentVariables;

namespace SME.SERAp.Prova.Dados
{
    public class RepositorioResultadoCicloEscola : RepositorioProvaSpBase, IRepositorioResultadoCicloEscola
    {
        public RepositorioResultadoCicloEscola(ConnectionStringOptions connectionStrings) : base(connectionStrings)
        {
        }

        public async Task<ResultadoCicloEscola> ObterResultadoCicloEscola(string edicao, int areaConhecimentoId, string uadSigla, string escCodigo, int cicloId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<long> IncluirAsync(ResultadoCicloEscola resultado)
        {
            throw new System.NotImplementedException();
        }

        public async Task<long> AlterarAsync(ResultadoCicloEscola resultado)
        {
            throw new System.NotImplementedException();
        }
    }
}