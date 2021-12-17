using SME.SERAp.Prova.Infra;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public interface IRabbitDeadletterSerapTratarUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagem);
    }
}
