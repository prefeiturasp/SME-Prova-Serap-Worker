using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Sentry;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarQuestoesProvaLegadoUseCase : ITratarQuestoesProvaLegadoUseCase
    {
        private readonly IMediator mediator;

        public TratarQuestoesProvaLegadoUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var questaoDto = mensagemRabbit.ObterObjetoMensagem<DetalheQuestaoDto>();
            try
            {
                var questao =
                await mediator.Send(
                    new ObterQuestaoDetalheLegadoPorIdQuery(questaoDto.ProvaLegadoId, questaoDto.QuestaoLegadoId));

                if (questao == null)
                    throw new Exception($"Questão {questaoDto.QuestaoLegadoId} não localizada!");

                var prova = await mediator.Send(new ObterProvaDetalhesPorIdQuery(questao.ProvaLegadoId));

                if (prova == null)
                    throw new Exception($"Prova {questao.ProvaLegadoId} não localizada!");

                var questaoParaPersistir = new Questao(
                    questao.Questao,
                    questao.QuestaoId,
                    questao.Enunciado,
                    questaoDto.QuestaoLegadoOrdem,
                    prova.Id,
                    (QuestaoTipo)questao.TipoItem
                );

                var questaoId = await mediator.Send(new QuestaoParaIncluirCommand(questaoParaPersistir));

                if (questaoParaPersistir.Arquivos != null && questaoParaPersistir.Arquivos.Any())
                {
                    questaoParaPersistir.Arquivos = await mediator.Send(new ObterTamanhoArquivosQuery(questaoParaPersistir.Arquivos));

                    foreach (var arquivoParaPersistir in questaoParaPersistir.Arquivos)
                    {
                        var arquivoId = await mediator.Send(new ArquivoPersistirCommand(arquivoParaPersistir));
                        await mediator.Send(new QuestaoArquivoPersistirCommand(questaoId, arquivoId));
                    }
                }

                var buscarPorProvaIdEQuestaoIdDto =
                    new BuscarPorProvaIdEQuestaoIdDto(questao.ProvaLegadoId, questao.QuestaoId);

                if (questao.TipoItem == (int)QuestaoTipo.MultiplaEscolha4Alternativas || questao.TipoItem == (int)QuestaoTipo.MultiplaEscolha5Alternativas)
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.AlternativaSync, buscarPorProvaIdEQuestaoIdDto));

            }
            catch(Exception ex)
            {
                SentrySdk.CaptureException(ex);
                return false;
            }

            return true;
        }
    }
}