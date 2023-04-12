using SME.SERAp.Prova.Dominio;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados.Interfaces
{
    public interface IRepositorioParticipacaoDre : IRepositorioProvaSpBase
    {
        Task<ParticipacaoDre> ObterParticipacaoDre(string edicao, string uad_sigla, string anoEscolar);
        Task<long> IncluirAsync(ParticipacaoDre resultado);
        Task<long> AlterarAsync(ParticipacaoDre resultado);
    }
}
