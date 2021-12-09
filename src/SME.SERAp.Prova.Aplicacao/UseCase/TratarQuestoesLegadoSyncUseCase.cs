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
            try
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
                        var alternativasLegadoId = await mediator.Send(new ObterAlternativasLegadoPorIdQuery(questaoSerap.QuestaoId));

                        foreach (var alternativaLegadoId in alternativasLegadoId)
                        {
                            try
                            {
                                var alternativa =
                           await mediator.Send(
                               new ObterAlternativaDetalheLegadoPorIdQuery(questaoSerap.QuestaoId, alternativaLegadoId));

                                if (alternativa == null)
                                    throw new Exception(
                                        $"A Alternativa {alternativa.AlternativaLegadoId} não localizada!");

                                var alternativaParaPersistir = new Alternativa(
                                alternativa.Ordem,
                                alternativa.Numeracao,
                                alternativa.Descricao,
                                questaoId);

                                var alternativaId = await mediator.Send(new AlternativaIncluirCommand(alternativaParaPersistir));

                                if (alternativaParaPersistir.Arquivos != null && alternativaParaPersistir.Arquivos.Any())
                                {
                                    alternativaParaPersistir.Arquivos = await mediator.Send(new ObterTamanhoArquivosQuery(alternativaParaPersistir.Arquivos));

                                    foreach (var arquivoParaPersistir in alternativaParaPersistir.Arquivos)
                                    {
                                        var arquivoId = await mediator.Send(new ArquivoPersistirCommand(arquivoParaPersistir));
                                        await mediator.Send(new AlternativaArquivoPersistirCommand(alternativaId, arquivoId));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                SentrySdk.CaptureException(ex);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
            return true;
        }
    }
}