using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class PropagarCacheProvasAnosUseCase : AbstractUseCase, IPropagarCacheProvasAnosUseCase
    {
        public PropagarCacheProvasAnosUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var provasAnos = await mediator.Send(new ObterProvasAnosDatasEModalidadesParaCacheQuery());

            if (provasAnos == null || !provasAnos.Any()) 
                return false;
            
            var minutosParaUmDia = (int)TimeSpan.FromDays(1).TotalMinutes;                
            await mediator.Send(new SalvarCacheCommand(CacheChave.ProvasAnosDatasEModalidades, provasAnos, minutosParaUmDia));

            return true;
        }
    }
}