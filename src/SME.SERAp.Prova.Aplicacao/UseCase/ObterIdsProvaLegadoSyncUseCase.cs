using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Aplicacao.Queries.ObterProvasLegadoSync;
using SME.SERAp.Prova.Dominio.Dtos;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterIdsProvaLegadoSyncUseCase : IObterIdsProvaLegadoSyncUseCase
    {
        private readonly IMediator mediator;


        public ObterIdsProvaLegadoSyncUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Executar(MensagemRabbit mensagemRabbit)
        {
            var resposta = mensagemRabbit.ObterObjetoMensagem<Teste>();
            Console.WriteLine(resposta?.Nome);
            var idsProva = await mediator.Send(new ObterIdsProvasLegadoSyncQuery());
            
            
        }
    }
}