using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarCadernoAlunoProvaTaiUseCase : ITratarCadernoAlunoProvaTaiUseCase
    {

        private readonly IMediator mediator;


        public TratarCadernoAlunoProvaTaiUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var alunoProva = mensagemRabbit.ObterObjetoMensagem<AlunoCadernoProvaTaiTratarDto>();
            var provaAtual = await mediator.Send(new ObterProvaDetalhesPorIdQuery(alunoProva.ProvaLegadoId));
            if (provaAtual == null)
                throw new Exception($"Prova {alunoProva.ProvaId} não localizada.");

            var dadosDaAmostraTai = await mediator.Send(new ObterDadosAmostraProvaTaiQuery(provaAtual.LegadoId));
            if (dadosDaAmostraTai == null)
                throw new NegocioException($"Os dados da amostra tai não foram cadastrados para  a prova {alunoProva.ProvaId}");

            var itens = await mediator.Send(new ObterItensAmostraTaiQuery(dadosDaAmostraTai.MatrizId, dadosDaAmostraTai.ListaConfigItens.Select(x => x.TipoCurriculoGradeId).ToArray()));
            if (itens == null || itens.Count() < dadosDaAmostraTai.NumeroItensAmostra)
                throw new NegocioException($"A quantidade de itens configurados com TRI é menor do que o número de itens para a prova {provaAtual.LegadoId}");

            var proficienciaAluno = await mediator.Send(new ObterProficienciaAlunoPorProvaIdQuery(provaAtual.Id, alunoProva.AlunoId));

            //Chamar Api TAI
            var itensTai = await mediator.Send(new ObterItensProvaTAISorteioRQuery(alunoProva.AlunoId, proficienciaAluno, itens, dadosDaAmostraTai.NumeroItensAmostra));
            var cadernoAluno = new CadernoAluno(
                    alunoProva.AlunoId,
                    provaAtual.Id,
                    alunoProva.AlunoId.ToString());

            var provaALuno = await mediator.Send(new CadernoAlunoIncluirCommand(cadernoAluno));

            var primeiraQuestao = true;
            foreach(var questao in itens.Where(t => itensTai.ItemAluno.Contains(t.ItemId)))
            {
                var questaoParaPersistir = new Questao(
                    questao.TextoBase,
                    questao.ItemId,
                    questao.Enunciado,
                    primeiraQuestao ? 0 : 999,
                    alunoProva.ProvaId,
                    (QuestaoTipo)questao.TipoItem,
                    alunoProva.AlunoId.ToString(),
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

                if (questaoParaPersistir.Tipo == QuestaoTipo.MultiplaEscolha)
                {
                    await TratarAlternativasQuestao(questao, questaoId);
                }

                primeiraQuestao = false;
            }

            return true;
        }
        private async Task TratarAlternativasQuestao(ItemAmostraTaiDto questaoSerap, long questaoId)
        {
            var alternativasLegadoId = await mediator.Send(new ObterAlternativasLegadoPorIdQuery(questaoSerap.ItemId));

            foreach (var alternativaLegadoId in alternativasLegadoId)
            {
                var alternativa = await mediator.Send(new ObterAlternativaDetalheLegadoPorIdQuery(questaoSerap.ItemId, alternativaLegadoId));

                if (alternativa == null)
                    throw new Exception($"A Alternativa {alternativa.AlternativaLegadoId} não localizada!");

                var alternativaParaPersistir = new Alternativa(
                alternativa.Ordem,
                alternativa.Numeracao,
                alternativa.Descricao,
                questaoId,
                alternativa.Correta);

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
    }
}
