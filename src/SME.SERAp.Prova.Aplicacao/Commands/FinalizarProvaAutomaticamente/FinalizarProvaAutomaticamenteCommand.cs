using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class FinalizarProvaAutomaticamenteCommand : IRequest<bool>
    {
        public ProvaParaAtualizarDto ProvaParaFinalizar { get; private set; }
        public FinalizarProvaAutomaticamenteCommand(ProvaParaAtualizarDto provaParaFinalizar)
        {
            ProvaParaFinalizar = provaParaFinalizar;
        }
    }
}
