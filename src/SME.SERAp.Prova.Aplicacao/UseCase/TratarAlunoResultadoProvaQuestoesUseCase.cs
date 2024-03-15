using MediatR;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.Interfaces;
using SME.SERAp.Prova.Infra;
using System;
using System.Threading.Tasks;
using SME.SERAp.Prova.Infra.Dtos;

namespace SME.SERAp.Prova.Aplicacao.UseCase
{
    public class TratarAlunoResultadoProvaQuestoesUseCase : ITratarAlunoResultadoProvaQuestoesUseCase
    {
        private readonly IMediator mediator;
        private readonly IServicoLog servicoLog;

        public TratarAlunoResultadoProvaQuestoesUseCase(IMediator mediator, IServicoLog servicoLog)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<ExportacaoAlunoProvaResultadoQuestaoDto>();
            
            try
            {
                if (filtro.ExportacaoResultado.Status == ExportacaoResultadoStatus.Processando)
                    await BuscaRespostasEIncluiConsolidado(filtro);
            }
            catch (Exception ex)
            {
                await mediator.Send(new ExportacaoResultadoAtualizarCommand(filtro.ExportacaoResultado, ExportacaoResultadoStatus.Erro));
                await mediator.Send(new ExcluirExportacaoResultadoItemCommand(0, filtro.ExportacaoResultado.Id));
                
                servicoLog.Registrar($"Escrever dados no arquivo CSV. msg: {mensagemRabbit.Mensagem}", ex);

                return false;
            }

            return true;
        }

        private async Task BuscaRespostasEIncluiConsolidado(ExportacaoAlunoProvaResultadoQuestaoDto filtro)
        {
            var respostas = await mediator.Send(new ObterQuestaoAlunoRespostaPorProvaIdEAlunoRaQuery(filtro.ConsolidadoProvaRespostaDto.ProvaSerapId, filtro.ConsolidadoProvaRespostaDto.AlunoCodigoEol)); // queryAdesao 

            foreach (var resposta in respostas)
            {
                var res = new ResultadoProvaConsolidado()
                {
                    AlunoCodigoEol = filtro.ConsolidadoProvaRespostaDto.AlunoCodigoEol,
                    AlunoDataNascimento = filtro.ConsolidadoProvaRespostaDto.AlunoDataNascimento,
                    AlunoFrequencia = filtro.ConsolidadoProvaRespostaDto.AlunoFrequencia,
                    AlunoNome = filtro.ConsolidadoProvaRespostaDto.AlunoNome,
                    AlunoSexo = filtro.ConsolidadoProvaRespostaDto.AlunoSexo,
                    DataFim = filtro.ConsolidadoProvaRespostaDto.DataFim,
                    DataInicio = filtro.ConsolidadoProvaRespostaDto.DataInicio,
                    DreCodigoEol = filtro.ConsolidadoProvaRespostaDto.DreCodigoEol,
                    DreNome = filtro.ConsolidadoProvaRespostaDto.DreNome,
                    DreSigla = filtro.ConsolidadoProvaRespostaDto.DreSigla,
                    ProvaCaderno = filtro.ConsolidadoProvaRespostaDto.ProvaCaderno,
                    ProvaComponente = filtro.ConsolidadoProvaRespostaDto.ProvaComponente,
                    ProvaQuantidadeQuestoes = filtro.ConsolidadoProvaRespostaDto.ProvaQuantidadeQuestoes,
                    ProvaSerapEstudantesId = filtro.ConsolidadoProvaRespostaDto.ProvaSerapEstudantesId,
                    ProvaSerapId = filtro.ConsolidadoProvaRespostaDto.ProvaSerapId,
                    TurmaAnoEscolar = filtro.ConsolidadoProvaRespostaDto.TurmaAnoEscolar,
                    TurmaAnoEscolarDescricao = filtro.ConsolidadoProvaRespostaDto.TurmaAnoEscolarDescricao,
                    TurmaCodigo = filtro.ConsolidadoProvaRespostaDto.TurmaCodigo,
                    TurmaDescricao = filtro.ConsolidadoProvaRespostaDto.TurmaDescricao,
                    UeCodigoEol = filtro.ConsolidadoProvaRespostaDto.UeCodigoEol,
                    UeNome = filtro.ConsolidadoProvaRespostaDto.UeNome,
                    QuestaoId = resposta.QuestaoId,
                    QuestaoOrdem = resposta.QuestaoOrdem,
                    Resposta = resposta.Resposta
                };

                await mediator.Send(new InserirResultadoProvaConsolidadoCommand(res));
            }
        }
    }
}

