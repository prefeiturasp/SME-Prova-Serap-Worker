using System.Threading.Tasks;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public interface IObterQuestoesProvaLegadoLegadoUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}
