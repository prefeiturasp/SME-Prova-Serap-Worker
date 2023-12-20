using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class PropagarCacheParametrosUseCase : AbstractUseCase, IPropagarCacheParametrosUseCase
    {
        public PropagarCacheParametrosUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var parametros = await mediator.Send(new ObterParametrosParaCacheQuery());

            if (parametros == null || !parametros.Any()) 
                return false;
            
            var minutosParaUmDia = (int)TimeSpan.FromDays(1).TotalMinutes;
            await mediator.Send(new SalvarCacheCommand(CacheChave.Parametros, parametros, minutosParaUmDia));

            return true;
        }
    }
}