using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao.Interfaces
{
    public interface IExecutarSincronizacaoInstitucionalTurmaSyncUseCase : IUseCase<MensagemRabbit, bool>
    {
    }
}