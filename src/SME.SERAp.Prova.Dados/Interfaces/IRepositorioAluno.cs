using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioAluno : IRepositorioBase<Aluno>
    {
        Task<Aluno> ObterAlunoPorCodigo(long codigo);
        Task<IEnumerable<Aluno>> ObterAlunosPorTurmaIdAsync(long turmaId);
    }
}
