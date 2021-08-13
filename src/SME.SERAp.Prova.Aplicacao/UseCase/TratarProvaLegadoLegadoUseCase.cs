using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Aplicacao.Queries.ObterProvasLegadoSync;
using SME.SERAp.Prova.Dominio.Dtos;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProvaLegadoLegadoUseCase : ITratarProvaLegadoLegadoUseCase
    {
        private readonly IMediator _mediator;

        public TratarProvaLegadoLegadoUseCase(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Executar(MensagemRabbit mensagemRabbit)
        {
            var resposta = mensagemRabbit.ObterObjetoMensagem<Teste>();
            Console.WriteLine(resposta.Nome);
        }
    }
}