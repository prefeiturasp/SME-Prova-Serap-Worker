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

            var turmaEolIds = new[] { filtro.TurmaEolId };

            try
            {
                IEnumerable<ConsolidadoAlunoProvaDto> consolidadoAlunosProva;
                if (filtro.AlunosComDeficiencia)
                    consolidadoAlunosProva = await mediator.Send(new ObterAlunosResultadoProvaDeficienciaQuery(filtro.ProvaSerapId, turmaEolIds));
                else if (filtro.AdesaoManual)
                    consolidadoAlunosProva = await mediator.Send(new ObterAlunosResultadoProvaAdesaoManualQuery(filtro.ProvaSerapId, turmaEolIds));
                else
                    consolidadoAlunosProva = await mediator.Send(new ObterAlunosResultadoProvaAdesaoTodosQuery(filtro.ProvaSerapId, turmaEolIds));

                if (consolidadoAlunosProva == null || !consolidadoAlunosProva.Any())
                    return false;

                foreach (var consolidadoAlunoProva in consolidadoAlunosProva)
                    await BuscaRespostasEIncluiConsolidado(consolidadoAlunoProva);

                var retorno = await AtualizarOuRemoverExportacaoResultadoItem(filtro.ProcessoId, filtro.DreEolId, filtro.TurmaEolId);
                
                if (retorno.removido)
                {
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ExtrairResultadosProva,
                        new ProvaExtracaoDto
                        {
                            ExtracaoResultadoId = filtro.ProcessoId,
                            ProvaSerapId = filtro.ProvaSerapId
                        }));
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
        
        private async Task BuscaRespostasEIncluiConsolidado(ConsolidadoAlunoProvaDto consolidadoAlunoProva)
        {
            var respostas = await mediator.Send(new ObterQuestaoAlunoRespostaPorProvaLegadoIdEAlunoRaQuery(
                consolidadoAlunoProva.ProvaSerapId, consolidadoAlunoProva.AlunoCodigoEol));

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

        private async Task<(bool atualizado, bool removido)> AtualizarOuRemoverExportacaoResultadoItem(long processoId, string dreCodigo, string turmaCodigoEol)
        {
            var exportacaoResultadoItem = await mediator.Send(new ObterExportacaoResultadoItemPorProcessoIdDreCodigoQuery(processoId, dreCodigo));
            if (exportacaoResultadoItem == null)
                return (atualizado: false, removido: false);

            var turmas = exportacaoResultadoItem.TurmaCodigoEol.Split(",");
            var listaTurmas = turmas.ToList();
            
            if (turmas.Contains($"'{turmaCodigoEol}'"))
                listaTurmas.Remove($"'{turmaCodigoEol}'");

            if (listaTurmas.Any())
            {
                var ues = exportacaoResultadoItem.UeCodigoEol.Split(",");
                var itemAlterado = new ExportacaoResultadoItem(exportacaoResultadoItem.ExportacaoResultadoId,
                    exportacaoResultadoItem.DreCodigoEol, ues, listaTurmas.ToArray());

                await mediator.Send(new AlterarExportacaoResultadoItemCommand(itemAlterado));
                return (atualizado: true, removido: false);
            }

            await mediator.Send(new ExcluirExportacaoResultadoItemCommand(exportacaoResultadoItem.Id, processoId));
            return (atualizado: false, removido: true);
        }
    }
}
