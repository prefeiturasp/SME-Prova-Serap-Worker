using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SERAp.Prova.Aplicacao.Queries;
using SME.SERAp.Prova.Infra.Exceptions;

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
            var provaLegadoId = long.Parse(mensagemRabbit.Mensagem.ToString() ?? string.Empty);
            
            var provaAtual = await mediator.Send(new ObterProvaDetalhesPorProvaLegadoIdQuery(provaLegadoId));
            if (provaAtual == null)
                throw new Exception($"Prova {provaLegadoId} não localizada!");

            if (provaAtual.FormatoTai)
                await TratarQuestoesTai(provaLegadoId, provaAtual.Id);
            else
                await TratarQuestoes(provaLegadoId, provaAtual.Id);

            return true;
        }

        private async Task TratarQuestoes(long provaLegadoId, long provaId)
        {
            var questoesSerap = await mediator.Send(new ObterQuestoesPorProvaIdQuery(provaLegadoId));
            foreach (var questaoSerap in questoesSerap)
            {
                var questaoParaPersistir = new Questao(
                    questaoSerap.TextoBase,
                    questaoSerap.QuestaoId,
                    questaoSerap.Enunciado,
                    questaoSerap.Ordem,
                    provaId,
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

                await TratarAudiosQuestao(questaoSerap.QuestaoId, questaoId);
                await TratarVideosQuestao(questaoSerap.QuestaoId, questaoId);

                if (questaoParaPersistir.Tipo == QuestaoTipo.MultiplaEscolha)
                    await TratarAlternativasQuestao(questaoSerap.QuestaoId, questaoId);
            }            
        }

        private async Task TratarQuestoesTai(long provaLegadoId, long provaId)
        {
            var dadosDaAmostraTai = await mediator.Send(new ObterDadosAmostraProvaTaiQuery(provaLegadoId));
            if (dadosDaAmostraTai == null || !dadosDaAmostraTai.Any())
                throw new NegocioException($"Os dados da amostra tai não foram cadastrados para a prova {provaLegadoId}.");
            
            var amostrasUtilizar = await ObterAmostrasUtilizar(dadosDaAmostraTai);
            if (!amostrasUtilizar.Any())
                throw new NegocioException($"Não há itens de amostra para serem utilizados na prova {provaLegadoId}.");
            
            var questoes = amostrasUtilizar.Distinct();
            foreach (var questao in questoes)
            {
                var questaoParaPersistir = new Questao(
                    questao.TextoBase,
                    questao.ItemId,
                    questao.Enunciado,
                    999,
                    provaId,
                    (QuestaoTipo)questao.TipoItem,
                    ProvaTai.Caderno,
                    questao.QuantidadeAlternativas,
                    questao.EixoId,
                    questao.HabilidadeId);

                var questaoId = await mediator.Send(new ObterIdQuestaoPorProvaIdCadernoLegadoIdQuery(provaId, ProvaTai.Caderno, questao.ItemId));
                if (questaoId == 0)
                    questaoId = await mediator.Send(new QuestaoParaIncluirCommand(questaoParaPersistir));

                if (questaoParaPersistir.Arquivos != null && questaoParaPersistir.Arquivos.Any())
                {
                    questaoParaPersistir.Arquivos =
                        await mediator.Send(new ObterTamanhoArquivosQuery(questaoParaPersistir.Arquivos));

                    foreach (var arquivoParaPersistir in questaoParaPersistir.Arquivos)
                    {
                        var arquivoId = await mediator.Send(new ObterIdArquivoPorCaminhoLegadoIdQuery(arquivoParaPersistir.Caminho, arquivoParaPersistir.LegadoId));
                        if (arquivoId == 0)
                            arquivoId = await mediator.Send(new ArquivoPersistirCommand(arquivoParaPersistir));

                        var questaoArquivoId =await mediator.Send(new ObterQuestaoArquivoIdPorQuestaoIdArquivoIdQuery(questaoId, arquivoId));
                        if (questaoArquivoId == 0)
                            await mediator.Send(new QuestaoArquivoPersistirCommand(questaoId, arquivoId));
                    }
                }

                await TratarAudiosQuestao(questao.ItemId, questaoId);
                await TratarVideosQuestao(questao.ItemId, questaoId);
                await TratarQuestaoTri(questao, questaoId);

                if (questaoParaPersistir.Tipo == QuestaoTipo.MultiplaEscolha)
                    await TratarAlternativasQuestao(questao.ItemId, questaoId);
            }
        }
        
        private async Task<List<ItemAmostraTaiDto>> ObterAmostrasUtilizar(IEnumerable<AmostraProvaTaiDto> dadosDaAmostraTai)
        {
            var amostrasUtilizar = new List<ItemAmostraTaiDto>();

            foreach (var dadosAmostra in dadosDaAmostraTai)
            {
                foreach (var configItem in dadosAmostra.ListaConfigItens)
                {
                    var itensAmostra = await mediator
                        .Send(new ObterItensAmostraTaiQuery(configItem.MatrizId, configItem.TipoCurriculoGradeId));
                    
                    var itensAmostraUtilizar = itensAmostra
                        .Where(c => !amostrasUtilizar.Select(x => x.ItemId).Contains(c.ItemId));
                    
                    amostrasUtilizar.AddRange(itensAmostraUtilizar);
                }
            }

            return amostrasUtilizar;
        }          

        private async Task TratarAlternativasQuestao(long questaoLegadoId, long questaoId)
        {
            var alternativasLegadoId = await mediator.Send(new ObterAlternativasLegadoPorIdQuery(questaoLegadoId));
            foreach (var alternativaLegadoId in alternativasLegadoId)
            {
                var alternativa = await mediator.Send(new ObterAlternativaDetalheLegadoPorIdQuery(questaoLegadoId, alternativaLegadoId));
                if (alternativa == null)
                    throw new Exception($"A Alternativa {alternativaLegadoId} não localizada!");

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

        private async Task TratarAudiosQuestao(long questaoLegadoId, long questaoId)
        {
            var audiosQuestao = await mediator.Send(new ObterAudiosPorQuestaoLegadoIdQuery(questaoLegadoId));
            if (audiosQuestao != null && audiosQuestao.Any())
            {
                audiosQuestao = await mediator.Send(new ObterTamanhoArquivosQuery(audiosQuestao));
                foreach (var audioParaPersistir in audiosQuestao)
                {
                    var arquivoId = await mediator.Send(new ObterIdArquivoPorCaminhoQuery(audioParaPersistir.Caminho));
                    if (arquivoId == 0)
                        arquivoId = await mediator.Send(new ArquivoPersistirCommand(audioParaPersistir));
                    
                    var questaoAudioId = await mediator.Send(new ObterQuestaoAudioIdPorArquivoIdQuery(questaoId, arquivoId));
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
    }
}