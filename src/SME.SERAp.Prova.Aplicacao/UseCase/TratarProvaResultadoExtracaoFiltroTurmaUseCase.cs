using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using SME.SERAp.Prova.Infra.Interfaces;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProvaResultadoExtracaoFiltroTurmaUseCase : ITratarProvaResultadoExtracaoFiltroTurmaUseCase 
    {
        private readonly IMediator mediator;
        private readonly IServicoLog servicoLog;

        public TratarProvaResultadoExtracaoFiltroTurmaUseCase(IMediator mediator, IServicoLog servicoLog)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<TratarProvaResultadoExtracaoFiltroTurmaDto>();
            if (filtro is null)
                throw new NegocioException("O filtro precisa ser informado");            
            
            var exportacaoResultado = await mediator.Send(new ObterExportacaoResultadoStatusQuery(filtro.ProcessoId, filtro.ProvaSerapId));
            if (exportacaoResultado is null)
                throw new NegocioException("A exportação não foi encontrada");
            
            if (!File.Exists(filtro.CaminhoArquivo))
                throw new NegocioException($"Arquivo não foi encontrado: {filtro.CaminhoArquivo}");            
            
            if (exportacaoResultado.Status != ExportacaoResultadoStatus.Processando) 
                return true;

            try
            {
                var turmaEolIds = new[] { filtro.TurmaEolId };
                var resultados = await mediator.Send(new ObterExtracaoProvaRespostaQuery(filtro.ProvaSerapId, filtro.DreEolId, filtro.UeEolId, turmaEolIds));
                if (resultados != null && resultados.Any())
                    await mediator.Send(new EscreverDadosCSVExtracaoProvaCommand(resultados, filtro.CaminhoArquivo));

                await RemoverExportacaoResultadoItem(filtro.ItemId);
                await FinalizarProcesso(filtro.ProcessoId, exportacaoResultado);
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
        
        private async Task RemoverExportacaoResultadoItem(long itemId)
        {
            var exportacaoResultadoItem = await mediator.Send(new ObterExportacaoResultadoItemPorIdQuery(itemId));
            if (exportacaoResultadoItem == null)
                return;

            await mediator.Send(new ExcluirExportacaoResultadoItemCommand(exportacaoResultadoItem.Id));
        } 
        
        private async Task FinalizarProcesso(long processoId, ExportacaoResultado exportacaoResultado)
        {
            var existeItemProcesso = await mediator.Send(new ConsultarSeExisteItemProcessoPorIdQuery(processoId));
            if (!existeItemProcesso)
                await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado, ExportacaoResultadoStatus.Finalizado));                
        }        
    }
}