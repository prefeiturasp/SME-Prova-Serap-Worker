using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ConsolidarProvaRespostaPorFiltroUseCase : IConsolidarProvaRespostaPorFiltroUseCase
    {
        private readonly IMediator mediator;
        private readonly IServicoLog serviceLog;

        public ConsolidarProvaRespostaPorFiltroUseCase(IMediator mediator, IServicoLog serviceLog)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.serviceLog = serviceLog ?? throw new ArgumentNullException(nameof(serviceLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<ExportacaoResultadoFiltroDto>();
            if (filtro is null)
                throw new NegocioException("O filtro precisa ser informado");            

            serviceLog.Registrar(LogNivel.Informacao, $"Consolidar dados prova por filtro. msg: {mensagemRabbit.Mensagem}");

            var exportacaoResultado = await mediator.Send(new ObterExportacaoResultadoStatusQuery(filtro.ProcessoId, filtro.ProvaSerapId));
            if (exportacaoResultado is null)
                throw new NegocioException("A exportação não foi encontrada");
            
            if (exportacaoResultado.Status != ExportacaoResultadoStatus.Processando)
                return true;            

            try
            {
                var turmas = await mediator.Send(new ObterTurmasPorCodigoUeEProvaSerapQuery(filtro.UeEolId, filtro.ProvaSerapId));
                if (turmas == null || !turmas.Any())
                    return false;

                var exportacaoResultadoItem = new ExportacaoResultadoItem(exportacaoResultado.Id, filtro.DreEolId, filtro.UeEolId);
                exportacaoResultadoItem.Id = await mediator.Send(new InserirExportacaoResultadoItemCommand(exportacaoResultadoItem));

                var turmasEolIds = turmas.Select(c => c.Codigo).ToArray();
                var exportacaoResultadoFiltroTurma = new ExportacaoResultadoFiltroTurmaDto(filtro.ProcessoId,
                    filtro.ProvaSerapId, exportacaoResultadoItem.Id, turmasEolIds,
                    filtro.CaminhoArquivo, filtro.AdesaoManual, filtro.AlunosComDeficiencia);
                
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ConsolidarProvaResultadoFiltroTurma, exportacaoResultadoFiltroTurma));
                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado, ExportacaoResultadoStatus.Erro));
                await mediator.Send(new ExcluirExportacaoResultadoItemCommand(0, exportacaoResultado.Id));
                serviceLog.Registrar($"Erro ao consolidar os dados da prova por filtro. msg: {mensagemRabbit.Mensagem}", ex);
                return false;
            }
        }
    }
}
