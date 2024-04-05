using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class PropagarCacheQuestoesCompletasProvaUseCase : AbstractUseCase, IPropagarCacheQuestoesCompletasProvaUseCase
    {
        public PropagarCacheQuestoesCompletasProvaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var mensagem = mensagemRabbit.Mensagem.ToString();
            
            if (string.IsNullOrEmpty(mensagem))
                return false;
            
            var provaId = long.Parse(mensagem);            
            
            if (provaId <= 0)
                return false;

            var questoesCompletas = await mediator.Send(new ObterQuestoesCompletasPorProvaIdParaCacheQuery(provaId));
            
            if (questoesCompletas == null || !questoesCompletas.Any())
                return false;

            foreach (var questaoCompleta in questoesCompletas)
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.PropagarCacheQuestoesCompletasProvaTratar, questaoCompleta.Id));

            return true;
        }
    }
}