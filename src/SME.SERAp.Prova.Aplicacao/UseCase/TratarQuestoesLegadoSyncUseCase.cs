using System;
using System.Collections.Generic;
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

                    await TratarAudiosQuestao(questaoSerap, questaoId);

                    await TratarVideosQuestao(questaoSerap.QuestaoId, questaoId);

                    if (questaoSerap.TipoItem == (int)QuestaoTipo.MultiplaEscolha)
                    {
                        await TratarAlternativasQuestao(questaoSerap, questaoId);
                    }
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
            return true;
        }

        private async Task TratarAlternativasQuestao(QuestoesPorProvaIdDto questaoSerap, long questaoId)
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

        private async Task TratarAudiosQuestao(QuestoesPorProvaIdDto questaoSerap, long questaoId)
        {
            var audiosQuestao = await mediator.Send(new ObterAudiosPorQuestaoLegadoIdQuery(questaoSerap.QuestaoId));
            if (audiosQuestao != null && audiosQuestao.Any())
            {
                audiosQuestao = await mediator.Send(new ObterTamanhoArquivosQuery(audiosQuestao));
                foreach (var audioParaPersistir in audiosQuestao)
                {
                    var arquivoId = await mediator.Send(new ArquivoPersistirCommand(audioParaPersistir));
                    await mediator.Send(new QuestaoAudioPersistirCommand(questaoId, arquivoId));
                }
            }
        }

        private async Task TratarVideosQuestao(long questaoLegadoId, long questaoId)
        {
            var videosQuestao = await mediator.Send(new ObterVideosPorQuestaoLegadoIdQuery(questaoLegadoId));
            if (videosQuestao != null && videosQuestao.Any())
            {                
                IEnumerable<Arquivo> videos = videosQuestao.Select(v => new Arquivo
                {
                    LegadoId = v.VideoId,
                    Caminho = v.CaminhoVideo
                });
                videos = await mediator.Send(new ObterTamanhoArquivosQuery(videos));

                IEnumerable<Arquivo> thumbnails = videosQuestao.Where(t => t.ThumbnailVideoId > 0)
                    .Select(t => new Arquivo
                {
                    LegadoId = t.ThumbnailVideoId,
                    Caminho = t.CaminhoThumbnailVideo
                });
                thumbnails = await mediator.Send(new ObterTamanhoArquivosQuery(thumbnails));

                IEnumerable<Arquivo> videosConvertido = videosQuestao.Where(vc => vc.VideoConvertidoId > 0)
                    .Select(vc => new Arquivo
                {
                    LegadoId = vc.VideoConvertidoId,
                    Caminho = vc.CaminhoVideoConvertido
                });
                videosConvertido = await mediator.Send(new ObterTamanhoArquivosQuery(videosConvertido));

                foreach (var video in videosQuestao)
                {
                    long arquivoVideoId = await mediator.Send(new ArquivoPersistirCommand(videos.FirstOrDefault(v => v.LegadoId == video.VideoId)));
                    
                    long? arquivoThumbnailId = null;
                    if(video.ThumbnailVideoId > 0)
                        arquivoThumbnailId = await mediator.Send(new ArquivoPersistirCommand(thumbnails.FirstOrDefault(t => t.LegadoId == video.ThumbnailVideoId)));

                    long? arquivoVideoConvertidoId = null;
                    if (video.VideoConvertidoId > 0)
                        arquivoVideoConvertidoId = await mediator.Send(new ArquivoPersistirCommand(videosConvertido.FirstOrDefault(vc => vc.LegadoId == video.VideoConvertidoId)));

                    var questaoVideoInserir = new QuestaoVideo(questaoId, arquivoVideoId, arquivoThumbnailId, arquivoVideoConvertidoId);

                    await mediator.Send(new QuestaoVideoPersistirCommand(questaoVideoInserir));
                }
            }
        }
    }
}