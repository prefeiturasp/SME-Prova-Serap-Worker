using SME.SERAp.Prova.Infra;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.Interfaces
{
    public interface IReabrirProvaAlunoUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}
