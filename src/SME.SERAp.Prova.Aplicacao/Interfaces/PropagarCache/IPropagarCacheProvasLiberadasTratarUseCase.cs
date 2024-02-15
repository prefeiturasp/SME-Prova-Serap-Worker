using System.Threading.Tasks;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public interface IPropagarCacheProvasLiberadasTratarUseCase
    {
        Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}