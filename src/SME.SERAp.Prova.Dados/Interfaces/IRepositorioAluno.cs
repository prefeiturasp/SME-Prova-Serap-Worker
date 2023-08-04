using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioAluno : IRepositorioBase<Aluno>
    {
        Task<Aluno> ObterAlunoPorCodigo(long codigo);
        Task<IEnumerable<Aluno>> ObterAlunosPorTurmaIdAsync(long turmaId);
        Task<long> InserirOuAtualizarAlunoAsync(AlunoEolDto alunoEol);
        Task<IEnumerable<Aluno>> ObterAlunosPorTurmasIdsAsync(long[] turmasIds);
        Task<IEnumerable<Aluno>> ObterTodosAsync();
        Task<IEnumerable<Aluno>> ObterAlunoPorCodigosAsync(long[] codigos);
        Task<IEnumerable<Aluno>> ObterAlunosAdesaoPorProvaId(long provaId);
        Task<IEnumerable<ProvaAlunoTaiSemCadernoDto>> ObterAlunosProvaTaiSemCadernoProvaId(long provaId);
        Task<IEnumerable<ProvaAlunoTaiSemCadernoDto>> ObterAlunosProvaTaiSemCaderno();
        Task<Aluno> ObterAlunoPorIdAsync(long alunoId);
        Task<bool> InativarAlunoPorIdETurmaIdAsync(long turmaId, long[] alunosId);
    }
}
