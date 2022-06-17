using MediatR;
using Sentry;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarItensAmostraProvaTaiUseCase : ITratarItensAmostraProvaTaiUseCase
    {

        private readonly IMediator mediator;

        public TratarItensAmostraProvaTaiUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {            
            try
            {
                var provaId = long.Parse(mensagemRabbit.Mensagem.ToString());

                var dadosDaAmostraTai = await mediator.Send(new ObterDadosAmostraProvaTaiQuery(provaId));
                var itens = await mediator.Send(new ObterItensAmostraTaiQuery(dadosDaAmostraTai.MatrizId, dadosDaAmostraTai.ListaConfigItens.Select(x => x.TipoCurriculoGradeId).ToArray()));

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
