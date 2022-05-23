using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioCadernoAluno : IRepositorioBase<CadernoAluno>
    {
        Task<IEnumerable<ProvaCadernoAlunoDto>> ObterAlunosSemCadernosProvaBibAsync();
        Task<bool> RemoverCadernosPorProvaIdAsync(long provaId);
    }
}
