using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Sentry;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarQuestoesLegadoSyncUseCase : ITratarQuestoesLegadoSyncUseCase
    {

        private readonly IMediator mediator;

        public TratarQuestoesLegadoSyncUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var provaId = long.Parse(mensagemRabbit.Mensagem.ToString());

            var questoesSerap = await mediator.Send(new ObterQuestoesPorProvaIdQuery(provaId));

            foreach (var questaoSerap in questoesSerap)
            {
                var prova = await mediator.Send(new ObterProvaDetalhesPorIdQuery(questaoSerap.ProvaLegadoId));

                if (prova == null)
                    throw new Exception($"Prova {provaId} não localizada!");

                var questaoParaPersistir = new Questao(
                    questaoSerap.TextoBase,
                    questaoSerap.QuestaoId,
                    questaoSerap.Enunciado,
                    questaoSerap.Ordem,
                    prova.Id,
                    (QuestaoTipo)questaoSerap.TipoItem,
                    questaoSerap.Caderno,
                    questaoSerap.QuantidadeAlternativas
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

                if (questaoSerap.TipoItem == (int)QuestaoTipo.MultiplaEscolha)
                {
                    var alternativasId = await mediator.Send(new ObterAlternativasLegadoPorIdQuery(questaoSerap.QuestaoId));

                    foreach (var alternativaId in alternativasId)
                    {
                        try
                        {
                            var alternativa =
                       await mediator.Send(
                           new ObterAlternativaDetalheLegadoPorIdQuery(questaoSerap.QuestaoId, alternativaId));

                            if (alternativa == null)
                                throw new Exception(
                                    $"A Alternativa {alternativa.AlternativaLegadoId} não localizada!");

                            var alternativaInserir = new Alternativa(
                            alternativa.Ordem,
                            alternativa.Numeracao,
                            alternativa.Descricao,
                            questaoId);

                            await mediator.Send(new AlternativaIncluirCommand(alternativaInserir));
                        }
                        catch (Exception ex)
                        {
                            SentrySdk.CaptureException(ex);
                        }
                       
                    }
                }
            }
            return true;
        }
    }
}