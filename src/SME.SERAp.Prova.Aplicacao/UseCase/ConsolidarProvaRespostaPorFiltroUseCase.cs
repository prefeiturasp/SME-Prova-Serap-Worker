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

            serviceLog.Registrar(LogNivel.Informacao, $"Consolidar dados prova por filtro. msg: {mensagemRabbit.Mensagem}");

            var exportacaoResultado = await mediator.Send(new ObterExportacaoResultadoStatusQuery(filtro.ProcessoId, filtro.ProvaId));
            try
            {
                if (filtro is null)
                    throw new NegocioException("O filtro precisa ser informado");

                if (exportacaoResultado is null)
                    throw new NegocioException("A exportação não foi encontrada");

                if (exportacaoResultado.Status != ExportacaoResultadoStatus.Processando)
                    return true;

                foreach (var ueCodigo in filtro.UeEolIds)
                {
                    var filtrosParaPublicar = new List<ExportacaoResultadoFiltroDto>();
                    var turmasUe = await mediator.Send(new ObterTurmasPorCodigoUeEProvaSerapQuery(ueCodigo, filtro.ProvaId));
                    
                    if (turmasUe != null && turmasUe.Any())
                    {
                        foreach (var turma in turmasUe)
                        {
                            var turmaEolIds = new[] { turma.Codigo };
                            var ueEolIds = new[] { ueCodigo };

                            var exportacaoResultadoItem = new ExportacaoResultadoItem(exportacaoResultado.Id, filtro.DreEolId, ueEolIds);
                            exportacaoResultadoItem.Id = await mediator.Send(new InserirExportacaoResultadoItemCommand(exportacaoResultadoItem));

                            var filtroDto = new ExportacaoResultadoFiltroDto(exportacaoResultado.Id,
                                exportacaoResultado.ProvaSerapId, exportacaoResultadoItem.Id, filtro.DreEolId, ueEolIds,
                                filtro.AdesaoManual, filtro.AlunosComDeficiencia)
                            {
                                TurmaEolIds = turmaEolIds
                            };
                            filtrosParaPublicar.Add(filtroDto);
                        }
                    }

                    if (!filtrosParaPublicar.Any()) 
                        continue;

                    foreach (var filtroPublicar in filtrosParaPublicar)
                        await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ConsolidarProvaResultadoFiltroTurma, filtroPublicar));
                }

                await mediator.Send(new ExcluirExportacaoResultadoItemCommand(filtro.ItemId));

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
