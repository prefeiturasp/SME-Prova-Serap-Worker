using SME.SERAp.Prova.Dominio;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados.Interfaces
{
    public interface IRepositorioParticipacaoSmeAreaConhecimento : IRepositorioProvaSpBase
    {
        Task<ParticipacaoSmeAreaConhecimento> ObterParticipacaoSmeAreaConhecimento(string edicao, int areaConhecimentoId, string anoEscolar);
        Task<long> IncluirAsync(ParticipacaoSmeAreaConhecimento resultado);
        Task<long> AlterarAsync(ParticipacaoSmeAreaConhecimento resultado);
    }
}
