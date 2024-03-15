using MediatR;
using RabbitMQ.Client;
using SME.SERAp.Prova.Aplicacao.Queries.ObterAlunosResultadoProvaAdesao;
using SME.SERAp.Prova.Aplicacao.Queries.ObterAlunosResultadoProvaDeficiencia;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Dtos;
using SME.SERAp.Prova.Infra.Exceptions;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ConsolidarProvaRespostaPorFiltroTurmaUseCase : IConsolidarProvaRespostaPorFiltroTurmaUseCase
    {
        
        private readonly IMediator mediator;
        private readonly IServicoLog servicoLog;
        private readonly IModel model;

        public ConsolidarProvaRespostaPorFiltroTurmaUseCase(IMediator mediator, IServicoLog servicoLog)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<ExportacaoResultadoFiltroDto>();
            var exportacaoResultado = await mediator.Send(new ObterExportacaoResultadoStatusQuery(filtro.ProcessoId, filtro.ProvaId));
      
            try
            {
                
                if (filtro is null)
                    throw new NegocioException("O filtro precisa ser informado");
                if (exportacaoResultado is null)
                    throw new NegocioException("A exportação não foi encontrada");


                if (exportacaoResultado.Status == ExportacaoResultadoStatus.Processando)
                {

                    //Excluir // Criar Command que Exclui que tem desses filtros. 
                    IEnumerable<ConsolidadoProvaRespostaDto> AlunosResultadoProva;


                    if (filtro.AdesaoManual)
                    {
                        AlunosResultadoProva = await mediator.Send(new ObterAlunosResultadoProvaAdesaoQuery(filtro.ProvaId)); // queryAdesao 
                        foreach (var alunoConsolidar in AlunosResultadoProva)
                        {

                            var consolidado = new ExportacaoAlunoProvaResultadoQuestaoDto()
                            {
                                ExportacaoResultado = exportacaoResultado,
                                ConsolidadoProvaRespostaDto = alunoConsolidar,
                                EhAdesaoATodos = true,
                                PossuiDeficiencia = false

                            };

                            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ConsolidarProvaResultadoFiltroTurmaTratar, consolidado));
                        }
                    }


                    else if (filtro.AlunosComDeficiencia)
                    {
                        AlunosResultadoProva = await mediator.Send(new ObterAlunosResultadoProvaDeficienciaQuery(filtro.ProvaId)); // queryAdesao 
                        AlunosResultadoProva = await mediator.Send(new ObterAlunosResultadoProvaAdesaoQuery(filtro.ProvaId)); // queryAdesao 
                        foreach (var alunoConsolidar in AlunosResultadoProva)
                        {

                            var consolidado = new ExportacaoAlunoProvaResultadoQuestaoDto()
                            {
                                ExportacaoResultado = exportacaoResultado,
                                ConsolidadoProvaRespostaDto = alunoConsolidar,
                                EhAdesaoATodos = false,
                                PossuiDeficiencia = true

                            };

                            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ConsolidarProvaResultadoFiltroTurmaTratar, consolidado));

                        }
                    }

                    else  // aderirATodos
                    {

                    }

                
        
                    //    await mediator.Send(new ConsolidarProvaRespostaPorFiltroCommand(filtro.ProvaId, filtro.DreEolId, filtro.UeEolIds, filtro.TurmaEolIds));
                    await mediator.Send(new ExcluirExportacaoResultadoItemCommand(filtro.ItemId));

                    bool existeItemProcesso = await mediator.Send(new ConsultarSeExisteItemProcessoPorIdQuery(exportacaoResultado.Id));
                    if (!existeItemProcesso)
                    {
                        var qtd = model.MessageCount(RotasRabbit.ConsolidarProvaResultadoFiltroTurmaTratar);
                        if (qtd == 0)
                        {
                            var extracao = new ProvaExtracaoDto { ExtracaoResultadoId = filtro.ProcessoId, ProvaSerapId = filtro.ProvaId };
                            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ExtrairResultadosProva, extracao));
                        }
                    
                    }
                  
                }

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado, ExportacaoResultadoStatus.Erro));
                await mediator.Send(new ExcluirExportacaoResultadoItemCommand(0, exportacaoResultado.Id));

                servicoLog.Registrar($"Erro ao consolidar os dados da prova por filtro. msg: {mensagemRabbit.Mensagem}", ex);
                return false;
            }

        }
    }
}
