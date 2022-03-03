using SME.SERAp.Prova.Dominio;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioAlunoDeficiencia : IRepositorioBase<AlunoDeficiencia>
    {
        Task<bool> RemoverPorAlunoRa(long alunoRa);
    }
}
