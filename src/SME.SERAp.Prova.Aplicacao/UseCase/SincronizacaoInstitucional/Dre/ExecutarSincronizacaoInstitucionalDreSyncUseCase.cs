using MediatR;
using Sentry;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExecutarSincronizacaoInstitucionalDreSyncUseCase : AbstractUseCase, IExecutarSincronizacaoInstitucionalDreSyncUseCase
    {
        public ExecutarSincronizacaoInstitucionalDreSyncUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var dres = await mediator.Send(new ObterDresSgpQuery());

            if (dres == null || !dres.Any())
            {
                throw new NegocioException("Não foi possível localizar as Dres no Sgp para a sincronização instituicional");
            }

            foreach (var dre in dres.Where(a => a.CodigoDre == "108800").ToList())
            {
                try
                {
                    var publicarTratamentoDre = await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.SincronizaEstruturaInstitucionalDreTratar, dre));
                    if (!publicarTratamentoDre)
                    {
                        var mensagem = $"Não foi possível inserir a Dre : {dre.Abreviacao} na fila de sync.";
                        SentrySdk.CaptureMessage(mensagem);
                    }
                }
                catch (Exception ex)
                {
                    SentrySdk.CaptureException(ex);
                }                
            }

            return true;
        }
    }
}
