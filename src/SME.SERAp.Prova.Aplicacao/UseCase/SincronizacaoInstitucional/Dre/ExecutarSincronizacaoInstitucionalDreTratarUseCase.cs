using MediatR;
using Newtonsoft.Json;
using Sentry;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExecutarSincronizacaoInstitucionalDreTratarUseCase : AbstractUseCase, IExecutarSincronizacaoInstitucionalDreTratarUseCase
    {
        public ExecutarSincronizacaoInstitucionalDreTratarUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var dreSgp = mensagemRabbit.ObterObjetoMensagem<Dre>();

            if (dreSgp == null)
                throw new NegocioException("Não foi possível localizar a Dre do Sgp.");

            var dreSerap = await mediator.Send(new ObterDrePorCodigoQuery(dreSgp.CodigoDre));

            try
            {
                var novoId = await mediator.Send(new TrataSincronizacaoInstitucionalDreCommand(dreSgp, dreSerap));
                if(novoId > 0)
                {
                    var dreParaPublicar = new DreParaSincronizacaoInstitucionalDto()
                    {
                        Id = novoId,
                        DreCodigo = dreSgp.CodigoDre
                    };
                    var mensagemParaPublicar = JsonConvert.SerializeObject(dreParaPublicar);
                    var publicarTratamentoDre = await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.SincronizaEstruturaInstitucionalUesSync, mensagemParaPublicar));
                    if (!publicarTratamentoDre)
                    {
                        var mensagem = $"Não foi possível inserir a Dre : {publicarTratamentoDre} na fila de sync.";
                        SentrySdk.CaptureMessage(mensagem);
                    }
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }

            return true;
        }
    }
}
