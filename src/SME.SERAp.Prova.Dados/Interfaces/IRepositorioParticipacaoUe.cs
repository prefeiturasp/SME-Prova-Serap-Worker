using SME.SERAp.Prova.Dominio;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioParticipacaoUe
    {
        Task<long> AlterarAsync(ParticipacaoUe participacao);
        Task<long> IncluirAsync(ParticipacaoUe participacao);
        Task<ParticipacaoUe> ObterParticipacaoUe(string edicao, string uad_sigla, string esc_codigo, string anoEscolar);
    }
}
