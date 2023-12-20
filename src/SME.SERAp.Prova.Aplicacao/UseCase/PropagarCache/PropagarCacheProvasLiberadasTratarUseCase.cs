using System;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class PropagarCacheProvasLiberadasTratarUseCase : AbstractUseCase, IPropagarCacheProvasLiberadasTratarUseCase
    {
        public PropagarCacheProvasLiberadasTratarUseCase(IMediator mediator) : base(mediator)
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

            var prova = await mediator.Send(new ObterProvaPorIdQuery(provaId));

            if (prova == null)
                return false;

            var chaveCache = string.Format(CacheChave.Prova, provaId);
            var minutosParaUmDia = (int)TimeSpan.FromDays(1).TotalMinutes;
            await mediator.Send(new SalvarCacheCommand(chaveCache, prova, minutosParaUmDia));
            
            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.PropagarCacheResumoQuestoesProva, provaId));
            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.PropagarCacheQuestoesCompletasProva, provaId));
            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.PropagarCacheQuestoesCompletasLegadoProva, provaId));
            
            return true;
        }
    }
}