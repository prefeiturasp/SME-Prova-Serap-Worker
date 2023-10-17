using MediatR;
using SME.SERAp.Prova.Aplicacao.Queries;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarCadernoAlunoProvaTaiUseCase : AbstractUseCase, ITratarCadernoAlunoProvaTaiUseCase
    {
        public TratarCadernoAlunoProvaTaiUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var alunoProva = mensagemRabbit.ObterObjetoMensagem<AlunoCadernoProvaTaiTratarDto>();

            var nomeChave = string.Format(CacheChave.SincronizandoProvaTaiAluno, alunoProva.ProvaId, alunoProva.AlunoId);
            try
            {
                var sincronizandoProvaAlunoTai = (bool)await mediator.Send(new ObterCacheQuery(nomeChave));
                if (sincronizandoProvaAlunoTai)
                    return false;

                await mediator.Send(new SalvarCacheCommandCommand(nomeChave, true));

                var caderno = alunoProva.AlunoId.ToString();

                var cadernoAluno = new CadernoAluno(
                    alunoProva.AlunoId,
                    alunoProva.ProvaId,
                    caderno);

                var existe = await mediator.Send(new ExisteCadernoAlunoPorProvaIdAlunoIdQuery(cadernoAluno.ProvaId, cadernoAluno.AlunoId));
                if (!existe)
                    await mediator.Send(new CadernoAlunoIncluirCommand(cadernoAluno));

                var primeiraQuestao = true;

                var questoes = alunoProva.ItensAmostra.Distinct();

                foreach (var questao in questoes)
                {
                    var questaoParaPersistir = new Questao(
                        questao.TextoBase,
                        questao.ItemId,
                        questao.Enunciado,
                        primeiraQuestao ? 0 : 999,
                        alunoProva.ProvaId,
                        (QuestaoTipo)questao.TipoItem,
                        caderno,
                        questao.QuantidadeAlternativas);

                    var questaoId = await mediator.Send(new ObterIdQuestaoPorProvaIdCadernoLegadoIdQuery(alunoProva.ProvaId, caderno, questao.ItemId));
                    if (questaoId == 0)
                        questaoId = await mediator.Send(new QuestaoParaIncluirCommand(questaoParaPersistir));

                    if (questaoParaPersistir.Arquivos != null && questaoParaPersistir.Arquivos.Any())
                    {
                        questaoParaPersistir.Arquivos = await mediator.Send(new ObterTamanhoArquivosQuery(questaoParaPersistir.Arquivos));

                        foreach (var arquivoParaPersistir in questaoParaPersistir.Arquivos)
                        {
                            var arquivoId = await mediator.Send(new ObterIdArquivoPorCaminhoQuery(arquivoParaPersistir.Caminho));
                            if (arquivoId == 0)
                                arquivoId = await mediator.Send(new ArquivoPersistirCommand(arquivoParaPersistir));

                            var questaoArquivoId = await mediator.Send(new ObterQuestaoArquivoIdPorQuestaoIdArquivoIdQuery(questaoId, arquivoId));
                            if (questaoArquivoId == 0)
                                await mediator.Send(new QuestaoArquivoPersistirCommand(questaoId, arquivoId));
                        }
                    }

                    await TratarAudiosQuestao(questao, questaoId);

                    await TratarVideosQuestao(questao.ItemId, questaoId);

                    await TratarQuestaoTri(questao, questaoId);

                    if (questaoParaPersistir.Tipo == QuestaoTipo.MultiplaEscolha)
                        await TratarAlternativasQuestao(questao, questaoId);

                    primeiraQuestao = false;
                }

                //-> Limpar o cache
                await RemoverQuestaoAmostraTaiAlunoCache(alunoProva.AlunoRa, alunoProva.ProvaId);
                await RemoverRespostaAmostraTaiAlunoCache(alunoProva.AlunoRa, alunoProva.ProvaId);
                await RemoverQuestaoProvaResumoCache(alunoProva.ProvaId, alunoProva.AlunoId);

                await mediator.Send(new RemoverCacheCommand(nomeChave));
                return true;
            }
            catch
            {
                await mediator.Send(new RemoverCacheCommand(nomeChave));
                throw;
            }
        }

        private async Task TratarAlternativasQuestao(ItemAmostraTaiDto questaoSerap, long questaoId)
        {
            var alternativasLegadoId = await mediator.Send(new ObterAlternativasLegadoPorIdQuery(questaoSerap.ItemId));

            foreach (var alternativaLegadoId in alternativasLegadoId)
            {
                var alternativa = await mediator.Send(new ObterAlternativaDetalheLegadoPorIdQuery(questaoSerap.ItemId, alternativaLegadoId));

                if (alternativa == null)
                    throw new Exception($"A alternativa {alternativaLegadoId} não localizada!");

                var alternativaParaPersistir = new Alternativa(
                    alternativa.AlternativaLegadoId,
                    alternativa.Ordem,
                    alternativa.Numeracao,
                    alternativa.Descricao,
                    questaoId,
                    alternativa.Correta);

                var alternativaId = await mediator.Send(new ObterIdAlternativaPorQuestaoIdLegadoIdQuery(questaoId, alternativa.AlternativaLegadoId));
                if (alternativaId == 0)
                    alternativaId = await mediator.Send(new AlternativaIncluirCommand(alternativaParaPersistir));

                if (alternativaParaPersistir.Arquivos == null || !alternativaParaPersistir.Arquivos.Any())
                    continue;

                alternativaParaPersistir.Arquivos = await mediator.Send(new ObterTamanhoArquivosQuery(alternativaParaPersistir.Arquivos));

                foreach (var arquivoParaPersistir in alternativaParaPersistir.Arquivos)
                {
                    var arquivoId = await mediator.Send(new ObterIdArquivoPorCaminhoQuery(arquivoParaPersistir.Caminho));
                    if (arquivoId == 0)
                        arquivoId = await mediator.Send(new ArquivoPersistirCommand(arquivoParaPersistir));

                    var alternativaArquivoId = await mediator.Send(new ObterAlternativaArquivoIdPorAlternativaIdArquivoIdQuery(alternativaId, arquivoId));
                    if (alternativaArquivoId == 0)
                        await mediator.Send(new AlternativaArquivoPersistirCommand(alternativaId, arquivoId));
                }
            }
        }

        private async Task TratarAudiosQuestao(ItemAmostraTaiDto questaoSerap, long questaoId)
        {
            var audiosQuestao = await mediator.Send(new ObterAudiosPorQuestaoLegadoIdQuery(questaoSerap.ItemId));

            if (audiosQuestao != null && audiosQuestao.Any())
            {
                audiosQuestao = await mediator.Send(new ObterTamanhoArquivosQuery(audiosQuestao));

                foreach (var audioParaPersistir in audiosQuestao)
                {
                    var arquivoId = await mediator.Send(new ObterIdArquivoPorCaminhoQuery(audioParaPersistir.Caminho));
                    if (arquivoId == 0)
                        arquivoId = await mediator.Send(new ArquivoPersistirCommand(audioParaPersistir));

                    var questaoAudioId = await mediator.Send(new ObterQuestaoAudioIdPorArquivoIdQuery(arquivoId));
                    if (questaoAudioId == 0)
                        await mediator.Send(new QuestaoAudioPersistirCommand(questaoId, arquivoId));
                }
            }
        }

        private async Task TratarVideosQuestao(long questaoLegadoId, long questaoId)
        {
            var videosQuestao = await mediator.Send(new ObterVideosPorQuestaoLegadoIdQuery(questaoLegadoId));

            if (videosQuestao != null && videosQuestao.Any())
            {
                var videos = videosQuestao.Select(v => new Arquivo
                {
                    LegadoId = v.VideoId,
                    Caminho = v.CaminhoVideo
                });

                videos = await mediator.Send(new ObterTamanhoArquivosQuery(videos));

                var thumbnails = videosQuestao.Where(t => t.ThumbnailVideoId > 0)
                    .Select(t => new Arquivo
                    {
                        LegadoId = t.ThumbnailVideoId,
                        Caminho = t.CaminhoThumbnailVideo
                    });

                thumbnails = await mediator.Send(new ObterTamanhoArquivosQuery(thumbnails));

                var videosConvertido = videosQuestao.Where(vc => vc.VideoConvertidoId > 0)
                    .Select(vc => new Arquivo
                    {
                        LegadoId = vc.VideoConvertidoId,
                        Caminho = vc.CaminhoVideoConvertido
                    });

                videosConvertido = await mediator.Send(new ObterTamanhoArquivosQuery(videosConvertido));

                foreach (var video in videosQuestao)
                {
                    var arquivoVideoId = await mediator.Send(new ObterIdArquivoPorCaminhoQuery(video.CaminhoVideo));
                    if (arquivoVideoId == 0)
                        arquivoVideoId = await mediator.Send(new ArquivoPersistirCommand(videos.FirstOrDefault(v => v.LegadoId == video.VideoId)));

                    long? arquivoThumbnailId = null;

                    if (video.ThumbnailVideoId > 0)
                    {
                        arquivoThumbnailId = await mediator.Send(new ObterIdArquivoPorCaminhoQuery(video.CaminhoVideo));
                        if (arquivoThumbnailId == 0)
                            arquivoThumbnailId = await mediator.Send(new ArquivoPersistirCommand(thumbnails.FirstOrDefault(t => t.LegadoId == video.ThumbnailVideoId)));
                    }

                    long? arquivoVideoConvertidoId = null;

                    if (video.VideoConvertidoId > 0)
                    {
                        arquivoVideoConvertidoId = await mediator.Send(new ObterIdArquivoPorCaminhoQuery(video.CaminhoVideo));
                        if (arquivoVideoConvertidoId == 0)
                            arquivoVideoConvertidoId = await mediator.Send(new ArquivoPersistirCommand(videosConvertido.FirstOrDefault(vc => vc.LegadoId == video.VideoConvertidoId)));
                    }

                    var questaoVideoId = await mediator.Send(new ObterQuestaoVideoIdPorQuestaoIdArquivoVideoIdQuery(questaoId, arquivoVideoId));
                    if (questaoVideoId > 0)
                        continue;

                    var questaoVideoInserir = new QuestaoVideo(questaoId, arquivoVideoId, arquivoThumbnailId, arquivoVideoConvertidoId);
                    await mediator.Send(new QuestaoVideoPersistirCommand(questaoVideoInserir));
                }
            }
        }

        private async Task TratarQuestaoTri(ItemAmostraTaiDto questaoSerap, long questaoId)
        {
            var questaoTri = await mediator.Send(new ObterQuestaoTriPorQuestaoIdQuery(questaoId));
            if (questaoTri != null)
                return;

            var questaoTriInserir = new QuestaoTri
            {
                QuestaoId = questaoId,
                Discriminacao = questaoSerap.Discriminacao,
                Dificuldade = questaoSerap.ProporcaoAcertos,
                AcertoCasual = questaoSerap.AcertoCasual
            };

            await mediator.Send(new IncluirAtualizarQuestaoTriCommand(questaoTriInserir));
        }

        private async Task RemoverQuestaoAmostraTaiAlunoCache(long alunoRa, long provaId)
        {
            await mediator.Send(new RemoverCacheCommand(string.Format(CacheChave.QuestaoAmostraTaiAluno,
                alunoRa, provaId)));
        }

        private async Task RemoverRespostaAmostraTaiAlunoCache(long alunoRa, long provaId)
        {
            await mediator.Send(new RemoverCacheCommand(string.Format(CacheChave.RespostaAmostraTaiAluno,
                alunoRa, provaId)));
        }

        private async Task RemoverQuestaoProvaResumoCache(long provaId, long alunoId)
        {
            await mediator.Send(new RemoverCacheCommand(string.Format(CacheChave.QuestaoProvaResumo,
                provaId, alunoId)));
        }
    }
}
