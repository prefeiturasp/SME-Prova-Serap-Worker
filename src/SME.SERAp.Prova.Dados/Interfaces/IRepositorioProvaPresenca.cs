using SME.SERAp.Prova.Dominio.Entidades.Presenca;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados.Interfaces
{
    public interface IRepositorioProvaPresenca : IRepositorioBase<ProvaPresenca>
    {
        Task<long> IncluirComTurmasAsync(ProvaPresenca provaPresenca, IEnumerable<long> turmasIds);
        Task<bool> AtualizarComTurmasAsync(ProvaPresenca provaPresenca, IEnumerable<long> turmasIds);
        Task<ProvaPresenca> ObterPorIdAsync(long id);
        Task<bool> DeletarAsync(long id);
    }
}