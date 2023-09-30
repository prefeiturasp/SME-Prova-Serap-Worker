using SME.SERAp.Prova.Infra;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.Interfaces
{
    public interface ITratarQuestaoCompletaProvaUseCase
    {
        public Task<bool> Executar(MensagemRabbit mensagemRabbit);
    }
}
