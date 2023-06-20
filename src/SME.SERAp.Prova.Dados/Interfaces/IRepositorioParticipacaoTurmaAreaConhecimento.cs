using SME.SERAp.Prova.Dominio;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados.Interfaces
{
    public interface IRepositorioParticipacaoTurmaAreaConhecimento : IRepositorioProvaSpBase
    {
        Task<ParticipacaoTurmaAreaConhecimento> ObterParticipacaoTurmaAreaConhecimento(string edicao, string uad_sigla, int areaConhecimentoId, string esc_codigo, string anoEscolar, string tur_codigo);
        Task<long> IncluirAsync(ParticipacaoTurmaAreaConhecimento participacao);
        Task<long> AlterarAsync(ParticipacaoTurmaAreaConhecimento participacao);
    }
}
