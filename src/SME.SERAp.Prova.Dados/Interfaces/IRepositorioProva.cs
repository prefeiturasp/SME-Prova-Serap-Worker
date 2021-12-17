using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioProva : IRepositorioBase<Dominio.Prova>
    {
        Task<Dominio.Prova> ObterPorIdLegadoAsync(long id);
        Task<bool> VerificaSeExisteRespostasPorId(long id);
        Task<bool> VerificaSeExistePorProvaSerapId(long provaId);
        Task CriarProvaRespostasExtracao(long provaId);
    }
}
