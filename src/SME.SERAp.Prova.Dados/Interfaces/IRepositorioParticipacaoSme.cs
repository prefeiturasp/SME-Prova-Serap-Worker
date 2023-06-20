using SME.SERAp.Prova.Dominio;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados.Interfaces
{
    public interface IRepositorioParticipacaoSme : IRepositorioProvaSpBase
    {
        Task<ParticipacaoSme> ObterParticipacaoSme(string edicao, string anoEscolar);
        Task<long> IncluirAsync(ParticipacaoSme resultado);
        Task<long> AlterarAsync(ParticipacaoSme resultado);
    }
}
