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
            var filtro = mensagemRabbit.ObterObjetoMensagem<ExportacaoResultadoFiltroTurmaDto>();
            if (filtro is null)
                throw new NegocioException("O filtro precisa ser informado");            
            
            var exportacaoResultado = await mediator.Send(new ObterExportacaoResultadoStatusQuery(filtro.ProcessoId, filtro.ProvaSerapId));
            if (exportacaoResultado is null)
                throw new NegocioException("A exportação não foi encontrada");
            
            if (exportacaoResultado.Status != ExportacaoResultadoStatus.Processando) 
                return true;

            try
            {
                IEnumerable<ConsolidadoAlunoProvaDto> consolidadoAlunosProva;
                if (filtro.AlunosComDeficiencia)
                    consolidadoAlunosProva = await mediator.Send(new ObterAlunosResultadoProvaDeficienciaQuery(filtro.ProvaSerapId, filtro.TurmasEolIds));
                else if (filtro.AdesaoManual)
                    consolidadoAlunosProva = await mediator.Send(new ObterAlunosResultadoProvaAdesaoManualQuery(filtro.ProvaSerapId, filtro.TurmasEolIds));
                else
                    consolidadoAlunosProva = await mediator.Send(new ObterAlunosResultadoProvaAdesaoTodosQuery(filtro.ProvaSerapId, filtro.TurmasEolIds));

                if (consolidadoAlunosProva == null || !consolidadoAlunosProva.Any())
                {
                    await RemoverExportacaoResultadoItem(filtro.ItemId);
                    await ExtrairResultados(filtro.ProcessoId, filtro.ProvaSerapId, filtro.CaminhoArquivo);
                    return false;
                }

                foreach (var consolidadoAlunoProva in consolidadoAlunosProva)
                    await BuscarRespostasEIncluirConsolidado(consolidadoAlunoProva);

                await RemoverExportacaoResultadoItem(filtro.ItemId);
                await ExtrairResultados(filtro.ProcessoId, filtro.ProvaSerapId, filtro.CaminhoArquivo);
                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new ExportacaoResultadoAtualizarCommand(exportacaoResultado, ExportacaoResultadoStatus.Erro));
                await mediator.Send(new ExcluirExportacaoResultadoItemCommand(0, exportacaoResultado.Id));

                servicoLog.Registrar($"Erro ao consolidar os dados da prova por turma. msg: {mensagemRabbit.Mensagem}", ex);
                return false;
            }
        }
        
        private async Task BuscarRespostasEIncluirConsolidado(ConsolidadoAlunoProvaDto consolidadoAlunoProva)
        {
            var respostas = await mediator.Send(new ObterQuestaoAlunoRespostaPorProvaLegadoIdEAlunoRaQuery(
                consolidadoAlunoProva.ProvaSerapId, consolidadoAlunoProva.AlunoCodigoEol, consolidadoAlunoProva.PossuiBib));

            foreach (var resposta in respostas)
            {
                var res = new ResultadoProvaConsolidado
                {
                    AlunoCodigoEol = consolidadoAlunoProva.AlunoCodigoEol,
                    AlunoDataNascimento = consolidadoAlunoProva.AlunoDataNascimento,
                    AlunoFrequencia = consolidadoAlunoProva.AlunoFrequencia,
                    AlunoNome = consolidadoAlunoProva.AlunoNome,
                    AlunoSexo = consolidadoAlunoProva.AlunoSexo,
                    DataFim = consolidadoAlunoProva.ProvaDataInicio,
                    DataInicio = consolidadoAlunoProva.ProvaDataEntregue,
                    DreCodigoEol = consolidadoAlunoProva.DreCodigoEol,
                    DreNome = consolidadoAlunoProva.DreNome,
                    DreSigla = consolidadoAlunoProva.DreSigla,
                    ProvaCaderno = consolidadoAlunoProva.ProvaCaderno,
                    ProvaComponente = consolidadoAlunoProva.ProvaComponente,
                    ProvaQuantidadeQuestoes = consolidadoAlunoProva.ProvaQuantidadeQuestoes,
                    ProvaSerapEstudantesId = consolidadoAlunoProva.ProvaSerapEstudantesId,
                    ProvaSerapId = consolidadoAlunoProva.ProvaSerapId,
                    TurmaAnoEscolar = consolidadoAlunoProva.TurmaAnoEscolar,
                    TurmaAnoEscolarDescricao = consolidadoAlunoProva.TurmaAnoEscolarDescricao,
                    TurmaCodigo = consolidadoAlunoProva.TurmaCodigo,
                    TurmaDescricao = consolidadoAlunoProva.TurmaDescricao,
                    UeCodigoEol = consolidadoAlunoProva.UeCodigoEol,
                    UeNome = consolidadoAlunoProva.UeNome,
                    QuestaoId = resposta.QuestaoId,
                    QuestaoOrdem = resposta.QuestaoOrdem,
                    Resposta = resposta.Resposta
                };

                await mediator.Send(new InserirResultadoProvaConsolidadoCommand(res));
            }
        }
        
        private async Task RemoverExportacaoResultadoItem(long itemId)
        {
            var exportacaoResultadoItem = await mediator.Send(new ObterExportacaoResultadoItemPorIdQuery(itemId));
            if (exportacaoResultadoItem == null)
                return;

            await mediator.Send(new ExcluirExportacaoResultadoItemCommand(exportacaoResultadoItem.Id));
        }

        private async Task ExtrairResultados(long processoId, long provaSerapId, string caminhoArquivo)
        {
            var existeItemProcesso = await mediator.Send(new ConsultarSeExisteItemProcessoPorIdQuery(processoId));
            if (!existeItemProcesso)
            {
                var extracao = new TratarProvaResultadoExtracaoDto { ExtracaoResultadoId = processoId, ProvaSerapId = provaSerapId, CaminhoArquivo = caminhoArquivo };
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ExtrairResultadosProva, extracao));
            }            
        }
    }
}
