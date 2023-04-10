using SME.SERAp.Prova.Dominio;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioParticipacaoUeAreaConhecimento
    {
        Task<long> AlterarAsync(ParticipacaoUeAreaConhecimento participacao);
        Task<long> IncluirAsync(ParticipacaoUeAreaConhecimento participacao);
        Task<ParticipacaoUeAreaConhecimento> ObterParticipacaoUeAreaConhecimento(string edicao, string uad_sigla, int areaConhecimentoId, string esc_codigo, string anoEscolar);
    }
}
