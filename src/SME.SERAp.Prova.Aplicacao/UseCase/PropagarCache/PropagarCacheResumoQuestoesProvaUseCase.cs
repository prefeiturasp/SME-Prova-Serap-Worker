using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class PropagarCacheResumoQuestoesProvaUseCase : AbstractUseCase, IPropagarCacheResumoQuestoesProvaUseCase
    {
        public PropagarCacheResumoQuestoesProvaUseCase(IMediator mediator) : base(mediator)
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

            var resumoQuestoes = await mediator.Send(new ObterResumoQuestoesPorProvaIdParaCacheQuery(provaId));

            if (resumoQuestoes == null || !resumoQuestoes.Any())
                return false;
            
            var chaveCache = string.Format(CacheChave.QuestaoProvaResumo, provaId);
            await mediator.Send(new SalvarCacheCommandCommand(chaveCache, resumoQuestoes));

            return true;
        }
    }
}