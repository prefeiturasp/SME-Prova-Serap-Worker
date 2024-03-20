using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
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

        public ConsolidarProvaRespostaPorFiltroTurmaUseCase(IMediator mediator, IServicoLog servicoLog)
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

                if (exportacaoResultado.Status != ExportacaoResultadoStatus.Processando) 
                    return true;

                IEnumerable<ConsolidadoAlunoProvaDto> consolidadoAlunosProva;
                if (filtro.AlunosComDeficiencia)
                    consolidadoAlunosProva = await mediator.Send(new ObterAlunosResultadoProvaDeficienciaQuery(filtro.ProvaSerapId, filtro.TurmaEolIds));
                else if (filtro.AdesaoManual)
                    consolidadoAlunosProva = await mediator.Send(new ObterAlunosResultadoProvaAdesaoManualQuery(filtro.ProvaSerapId, filtro.TurmaEolIds));
                else
                    consolidadoAlunosProva = await mediator.Send(new ObterAlunosResultadoProvaAdesaoTodosQuery(filtro.ProvaSerapId, filtro.TurmaEolIds));

                if (consolidadoAlunosProva != null && consolidadoAlunosProva.Any())
                {
                    foreach (var consolidadoAlunoProva in consolidadoAlunosProva)
                    {
                        var respostas = await mediator.Send(new ObterQuestaoAlunoRespostaPorProvaLegadoIdEAlunoRaQuery(consolidadoAlunoProva.ProvaSerapId, consolidadoAlunoProva.AlunoCodigoEol));
                        if (respostas == null || !respostas.Any())
                            continue;

                        foreach (var resposta in respostas)
                        {
                            var res = new ResultadoProvaConsolidado
                            {
                                ProvaSerapId = consolidadoAlunoProva.ProvaSerapId,
                                ProvaSerapEstudantesId = consolidadoAlunoProva.ProvaSerapEstudantesId,
                                AlunoCodigoEol = consolidadoAlunoProva.AlunoCodigoEol,
                                AlunoNome = consolidadoAlunoProva.AlunoNome,
                                AlunoSexo = consolidadoAlunoProva.AlunoSexo,
                                AlunoDataNascimento = consolidadoAlunoProva.AlunoDataNascimento,
                                ProvaComponente = consolidadoAlunoProva.ProvaComponente,
                                ProvaCaderno = consolidadoAlunoProva.ProvaCaderno,
                                ProvaQuantidadeQuestoes = consolidadoAlunoProva.ProvaQuantidadeQuestoes,
                                AlunoFrequencia = consolidadoAlunoProva.AlunoFrequencia,
                                DataInicio = consolidadoAlunoProva.ProvaDataInicio,
                                DataFim = consolidadoAlunoProva.ProvaDataEntregue,
                                DreCodigoEol = consolidadoAlunoProva.DreCodigoEol,
                                DreSigla = consolidadoAlunoProva.DreSigla,
                                DreNome = consolidadoAlunoProva.DreNome,
                                UeCodigoEol = consolidadoAlunoProva.UeCodigoEol,
                                UeNome = consolidadoAlunoProva.UeNome,
                                TurmaAnoEscolar = consolidadoAlunoProva.TurmaAnoEscolar,
                                TurmaAnoEscolarDescricao = consolidadoAlunoProva.TurmaAnoEscolarDescricao,
                                TurmaCodigo = consolidadoAlunoProva.TurmaCodigo,
                                TurmaDescricao = consolidadoAlunoProva.TurmaDescricao,
                                QuestaoId = resposta.QuestaoId,
                                QuestaoOrdem = resposta.QuestaoOrdem,
                                Resposta = resposta.Resposta
                            };
                            
                            await mediator.Send(new InserirResultadoProvaConsolidadoCommand(res));                                    
                        }
                    }
                }
                
                await mediator.Send(new ExcluirExportacaoResultadoItemCommand(filtro.ItemId));

                var existeItemProcesso = await mediator.Send(new ConsultarSeExisteItemProcessoPorIdQuery(exportacaoResultado.Id));
                if (existeItemProcesso)
                    return true;

                var extracao = new ProvaExtracaoDto { ExtracaoResultadoId = filtro.ProcessoId, ProvaSerapId = filtro.ProvaSerapId };
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ExtrairResultadosProva, extracao));

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
