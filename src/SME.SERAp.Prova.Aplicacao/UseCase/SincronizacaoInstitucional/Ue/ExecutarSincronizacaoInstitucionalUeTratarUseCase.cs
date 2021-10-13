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
    public class ExecutarSincronizacaoInstitucionalUeTratarUseCase : AbstractUseCase, IExecutarSincronizacaoInstitucionalUeTratarUseCase
    {
        public ExecutarSincronizacaoInstitucionalUeTratarUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var ueSgp = mensagemRabbit.ObterObjetoMensagem<Ue>();

            if (ueSgp == null)
                throw new NegocioException("Não foi possível localizar a Ue do Sgp.");

            var ueSerap = await mediator.Send(new ObterUePorCodigoQuery(ueSgp.CodigoUe));

            try
            {
                var ueNovoId = await mediator.Send(new TrataSincronizacaoInstitucionalUeCommand(ueSgp, ueSerap));
                if (ueNovoId > 0)
                {
                    var ueParaPublicar = new UeParaSincronizacaoInstitucionalDto()
                    {
                        Id = ueNovoId,
                        UeCodigo = ueSgp.CodigoUe
                    };
                    var mensagemParaPublicar = JsonConvert.SerializeObject(ueParaPublicar);
                    var publicarTratamentoUe = await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.SincronizaEstruturaInstitucionalTurmasSync, mensagemParaPublicar));
                    if (!publicarTratamentoUe)
                    {
                        var mensagem = $"Não foi possível inserir a Ue : {publicarTratamentoUe} na fila de sync.";
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
