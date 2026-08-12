using SME.SERAp.Prova.Infra;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.Interfaces
{
    public interface IDeletarProvaPresencaUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}