using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioCadernoAluno : IRepositorioBase<CadernoAluno>
    {
        Task<bool> ExisteCadernoAlunoPorProvaIdAlunoId(long provaId, long alunoId);
        Task<IEnumerable<ProvaCadernoAlunoDto>> ObterAlunosSemCadernosProvaBibAsync(long provaId);
        Task<CadernoAluno> ObterCadernoAlunoPorProvaIdAlunoIdAsync(long provaId, long alunoId);
        Task<bool> RemoverCadernosPorProvaIdAsync(long provaId);
    }
}
