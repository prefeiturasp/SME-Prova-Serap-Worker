using MediatR;
using Sentry;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProvaResultadoExtracaoUseCase : ITratarProvaResultadoExtracaoUseCase
    {
        private readonly IMediator mediator;

        public TratarProvaResultadoExtracaoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var extracao = mensagemRabbit.ObterObjetoMensagem<ProvaExtracaoDto>();
            var exportacaoResultado = await mediator.Send(new ObterExportacaoResultadoStatusQuery(extracao.ExtracaoResultadoId, extracao.ProvaSerapId));
            try
            {
                if (extracao is null)
                    throw new NegocioException("O id da prova serap precisa ser informado");

                if (exportacaoResultado is null)
                    throw new NegocioException("A exportação não foi encontrada");

                var checarProvaExiste = await mediator.Send(new VerificaProvaExistePorSerapIdQuery(extracao.ProvaSerapId));

                if (!checarProvaExiste)
                    throw new NegocioException("A prova informada não foi encontrada no serap estudantes");

                var resultado = new List<ConsolidadoProvaRespostaDto>();
                var dres = await mediator.Send(new ObterDresSerapQuery());
                foreach (Dre dre in dres)
                {
                    var resultadoDre = await mediator.Send(new ObterExtracaoProvaRespostaQuery(extracao.ProvaSerapId, dre.CodigoDre));
                    if (resultadoDre != null && resultadoDre.Any())
                        resultado.AddRange(resultadoDre.ToList());
                }

                if (!resultado.Any())
                    throw new NegocioException($"Os resultados da prova {extracao.ProvaSerapId} ainda não foram gerados");

                if (exportacaoResultado.Status == ExportacaoResultadoStatus.Processando)
                {
                    exportacaoResultado = await mediator.Send(new ObterExportacaoResultadoPorIdQuery(exportacaoResultado.Id));
                    await mediator.Send(new GerarCSVExtracaoProvaCommand(resultado, exportacaoResultado.NomeArquivo));
                    await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado, ExportacaoResultadoStatus.Finalizado));
                    await mediator.Send(new ExcluirExportacaoResultadoItemCommand(0, exportacaoResultado.Id));
                }
            }
            catch (Exception ex)
            {
                await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado, ExportacaoResultadoStatus.Erro));
                SentrySdk.CaptureMessage($"Erro ao gerar CSV da prova. msg: {mensagemRabbit.Mensagem}", SentryLevel.Error);
                SentrySdk.CaptureException(ex);
                throw ex;
            }
            return true;
        }
    }
}