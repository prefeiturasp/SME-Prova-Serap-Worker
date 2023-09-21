using MediatR;
using SME.SERAp.Prova.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.Dtos.Tai;
using SME.SERAp.Prova.Infra.Exceptions;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarCadernosProvaTaiUseCase : AbstractUseCase, ITratarCadernosProvaTaiUseCase
    {
        public TratarCadernosProvaTaiUseCase(IMediator mediator) : base(mediator)
        {
        }        
        
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var cadernoProvaTaiTratar = mensagemRabbit.ObterObjetoMensagem<CadernoProvaTaiTratarDto>();

                if (cadernoProvaTaiTratar.ProvaId == 0)
                    throw new NegocioException("O Id da prova deve ser informado.");
                
                if (cadernoProvaTaiTratar.ProvaLegadoId == 0)
                    throw new NegocioException("O Id da prova do legado deve ser informado.");

                await InserirQuestoesAmostra(cadernoProvaTaiTratar);
                await PublicarFilaTratarCadernoAluno(cadernoProvaTaiTratar);

                return true;
            }
            catch (Exception ex)
            {
                throw new ErroException($"Erro ao tratar cadernos da prova TAI: {ex.Message}");
            }
        }

        private async Task InserirQuestoesAmostra(CadernoProvaTaiTratarDto cadernoProvaTaiTratar)
        {
            var primeiraQuestao = true;
            
            foreach (var questao in cadernoProvaTaiTratar.ItensAmostra)
            {
                var questaoParaPersistir = new Questao(
                    questao.TextoBase,
                    questao.ItemId,
                    questao.Enunciado,
                    primeiraQuestao ? 0 : 999,
                    cadernoProvaTaiTratar.ProvaId,
                    (QuestaoTipo)questao.TipoItem,
                    cadernoProvaTaiTratar.Caderno,
                    questao.QuantidadeAlternativas);

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

                await TratarAudiosQuestao(questao, questaoId);

                await TratarVideosQuestao(questao.ItemId, questaoId);

                await TratarQuestaoTri(questao, questaoId);

                if (questaoParaPersistir.Tipo == QuestaoTipo.MultiplaEscolha)
                    await TratarAlternativasQuestao(questao, questaoId);
                
                primeiraQuestao = false;                
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
                    var arquivoVideoId = await mediator.Send(new ArquivoPersistirCommand(videos.FirstOrDefault(v => v.LegadoId == video.VideoId)));

                    long? arquivoThumbnailId = null;
                    
                    if (video.ThumbnailVideoId > 0)
                        arquivoThumbnailId = await mediator.Send(new ArquivoPersistirCommand(thumbnails.FirstOrDefault(t => t.LegadoId == video.ThumbnailVideoId)));

                    long? arquivoVideoConvertidoId = null;

                    if (video.VideoConvertidoId > 0)
                        arquivoVideoConvertidoId = await mediator.Send(new ArquivoPersistirCommand(videosConvertido.FirstOrDefault(vc => vc.LegadoId == video.VideoConvertidoId)));

                    var questaoVideoInserir = new QuestaoVideo(questaoId, arquivoVideoId, arquivoThumbnailId, arquivoVideoConvertidoId);

                    await mediator.Send(new QuestaoVideoPersistirCommand(questaoVideoInserir));
                }
            }
        }

        private async Task TratarQuestaoTri(ItemAmostraTaiDto questaoSerap, long questaoId)
        {
            var questaoTriInserir = new QuestaoTri
            {
                QuestaoId = questaoId,
                Discriminacao = questaoSerap.Discriminacao,
                Dificuldade = questaoSerap.ProporcaoAcertos,
                AcertoCasual = questaoSerap.AcertoCasual
            };

            await mediator.Send(new IncluirAtualizarQuestaoTriCommand(questaoTriInserir));
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

                var alternativaId = await mediator.Send(new AlternativaIncluirCommand(alternativaParaPersistir));

                if (alternativaParaPersistir.Arquivos == null || !alternativaParaPersistir.Arquivos.Any()) 
                    continue;
                
                alternativaParaPersistir.Arquivos = await mediator.Send(new ObterTamanhoArquivosQuery(alternativaParaPersistir.Arquivos));

                foreach (var arquivoParaPersistir in alternativaParaPersistir.Arquivos)
                {
                    var arquivoId = await mediator.Send(new ArquivoPersistirCommand(arquivoParaPersistir));
                    await mediator.Send(new AlternativaArquivoPersistirCommand(alternativaId, arquivoId));
                }
            }
        }        

        private async Task PublicarFilaTratarCadernoAluno(CadernoProvaTaiTratarDto cadernoProvaTaiTratar)
        {
            var alunosAtivosProvaTaiSemCaderno = cadernoProvaTaiTratar.AlunosProvaTaiSemCaderno
                .Where(a => a.Ativo());

            foreach (var item in alunosAtivosProvaTaiSemCaderno)
            {
                var msg = new AlunoCadernoProvaTaiTratarDto(item.ProvaId, item.AlunoId, item.ProvaLegadoId,
                    item.AlunoRa, cadernoProvaTaiTratar.Disciplina, cadernoProvaTaiTratar.ItensAmostra,
                    cadernoProvaTaiTratar.NumeroItensAmostra, cadernoProvaTaiTratar.Ano,
                    cadernoProvaTaiTratar.Caderno);
                
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TratarCadernoAlunoProvaTai, msg));
            }
        }
    }
}
