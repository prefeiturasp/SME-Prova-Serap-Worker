using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProvaResultadoExtracaoFiltroUseCase : ITratarProvaResultadoExtracaoFiltroUseCase
    {
        private readonly IMediator mediator;
        private readonly IServicoLog servicoLog;

        public TratarProvaResultadoExtracaoFiltroUseCase(IMediator mediator, IServicoLog servicoLog)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<ExportacaoResultadoFiltroDto>();
            
            var exportacaoResultado = await mediator.Send(new ObterExportacaoResultadoStatusQuery(filtro.ProcessoId, filtro.ProvaSerapId));
            if (exportacaoResultado is null)
                throw new NegocioException("A exportação não foi encontrada");

            try 
            {
                if (filtro is null)
                    throw new NegocioException("O filtro precisa ser informado");

                if (exportacaoResultado.Status == ExportacaoResultadoStatus.Processando)
                {
                    if (!File.Exists(filtro.CaminhoArquivo))
                        throw new NegocioException($"Arquivo não foi encontrado: {filtro.CaminhoArquivo}");

                    var resultados = await mediator.Send(new ObterExtracaoProvaRespostaQuery(filtro.ProvaSerapId, !filtro.AdesaoManual));
                    if (resultados != null && resultados.Any())
                        await mediator.Send(new EscreverDadosCSVExtracaoProvaCommand(resultados, filtro.CaminhoArquivo));

                    await mediator.Send(new ExcluirExportacaoResultadoItemCommand(filtro.ItemId));

                    var existeItemProcesso = await mediator.Send(new ConsultarSeExisteItemProcessoPorIdQuery(exportacaoResultado.Id));
                    if (!existeItemProcesso)
                        await mediator.Send(new ExcluirExportacaoResultadoItemCommand(0, exportacaoResultado.Id));
                }
            }
            catch (Exception ex)
            {
                await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado, ExportacaoResultadoStatus.Erro));
                await mediator.Send(new ExcluirExportacaoResultadoItemCommand(0, exportacaoResultado.Id));

                servicoLog.Registrar($"Escrever dados no arquivo CSV. msg: {mensagemRabbit.Mensagem}", ex);

                return false;
            }

            return true;
        }
    }
}
