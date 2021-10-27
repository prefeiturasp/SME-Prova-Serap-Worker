using MediatR;
using Newtonsoft.Json;
using Sentry;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExecutarSincronizacaoInstitucionalTurmaTratarUseCase : AbstractUseCase, IExecutarSincronizacaoInstitucionalTurmaTratarUseCase
    {
        public ExecutarSincronizacaoInstitucionalTurmaTratarUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var turmaSgp = mensagemRabbit.ObterObjetoMensagem<TurmaSgpDto>();

            if (turmaSgp == null) return false;

            try
            {
                var turmaSerap = await mediator.Send(new ObterTurmaPorCodigoQuery(turmaSgp.Codigo));

                var turmaTratadaId = await mediator.Send(new TrataSincronizacaoInstitucionalTurmaCommand(turmaSgp, turmaSerap));

                turmaSgp.TurmaId = turmaTratadaId;
                var mensagemParaPublicar = JsonConvert.SerializeObject(turmaSgp);

                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.SincronizaEstruturaInstitucionalAlunoSync, mensagemParaPublicar));
                

            }
            catch (Exception ex)
            {
                SentrySdk.CaptureMessage($"Não foi possível realizar o tratamento da turma id {turmaSgp.Codigo}.");
                SentrySdk.CaptureException(ex);
                throw;
            }
            return true;
        }
    }
}
