using System.Threading.Tasks;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public interface IPropagarCacheUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);        
    }
}