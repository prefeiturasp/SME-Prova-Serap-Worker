using MediatR;
using Sentry;
using SME.SERAp.Prova.Aplicacao;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ConsolidarProvaRespostaPorFiltroUseCase : IConsolidarProvaRespostaPorFiltroUseCase
    {
        private readonly IMediator mediator;
        public ConsolidarProvaRespostaPorFiltroUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<ConsolidarProvaFiltroDto>();
            var exportacaoResultado = await mediator.Send(new ObterExportacaoResultadoStatusQuery(filtro.ProcessoId, filtro.ProvaId));
            try
            {
                if (filtro is null)
                    throw new NegocioException("O filtro precisa ser informado");
                if (exportacaoResultado is null)
                    throw new NegocioException("A exportação não foi encontrada");

                if (exportacaoResultado.Status == ExportacaoResultadoStatus.Processando)
                {
                    await mediator.Send(new ConsolidarProvaRespostaPorFiltroCommand(filtro.ProvaId, filtro.DreEolId, filtro.UeEolIds));
                    if (filtro.FinalizarProcesso)
                    {
                        var extracao = new ProvaExtracaoDto { ExtracaoResultadoId = filtro.ProcessoId, ProvaSerapId = filtro.ProvaId };
                        await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ExtrairResultadosProva, extracao));
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado, ExportacaoResultadoStatus.Erro));
                SentrySdk.CaptureMessage($"Erro ao consolidar os dados da prova por filtro. msg: {mensagemRabbit.Mensagem}", SentryLevel.Error);
                SentrySdk.CaptureException(ex);
                return false;
            }
            
        }
    }
}
