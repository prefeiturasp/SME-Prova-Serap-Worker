using MediatR;
using Newtonsoft.Json;
using Sentry;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExecutarSincronizacaoInstitucionalTurmaSyncUseCase : AbstractUseCase, IExecutarSincronizacaoInstitucionalTurmaSyncUseCase
    {
        public ExecutarSincronizacaoInstitucionalTurmaSyncUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var ue = mensagemRabbit.ObterObjetoMensagem<UeParaSincronizacaoInstitucionalDto>();
            if (ue == null)
            {
                var mensagem = $"Não foi possível inserir a Ue : {ue.UeCodigo} na fila de sync.";
                SentrySdk.CaptureMessage(mensagem);
                throw new NegocioException(mensagem);
            }

            var turmasSgp = await mediator.Send(new ObterTurmasSgpPorUeIdQuery(ue.Id));
            if (!turmasSgp?.Any() ?? false) return false;

            foreach (var turma in turmasSgp.Where(t => t.Ano == "4"))
            {
                try
                {
                    turma.UeId = ue.Id;
                    var mensagemParaPublicar = JsonConvert.SerializeObject(turma);

                    var publicarFilaIncluirTurma = await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.SincronizaEstruturaInstitucionalTurmaTratar, mensagemParaPublicar));
                    if (!publicarFilaIncluirTurma)
                    {
                        var mensagem = $"Não foi possível inserir a turma de codígo : {turma.Codigo} na fila de inclusão.";
                        SentrySdk.CaptureMessage(mensagem);
                    }
                }
                catch (Exception ex)
                {
                    SentrySdk.AddBreadcrumb($"Não foi possível incluir a turma {turma.Codigo} na fila para tratamento", "sincronizacao-institucional", null, null);
                    SentrySdk.CaptureException(ex);
                }                
            }
            return true;
        }
    }
}
