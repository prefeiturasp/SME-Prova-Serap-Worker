using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao.Interfaces
{
    public interface IExecutarSincronizacaoInstitucionalAlunoSyncUseCase : IUseCase<MensagemRabbit, bool>
    {
    }
}