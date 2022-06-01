using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioAlunoProvaProficiencia : IRepositorioBase<AlunoProvaProficiencia>
    {
        Task<IEnumerable<AlunoProvaDto>> ObterAlunosSemProficienciaAsync();
        Task<bool> ExisteAsync(long alunoId, long provaId);
        Task<decimal> ObterUltimaProficienciaAlunoPorDisciplinaIdAsync(long alunoId, long? disciplinaId);
    }
}
