using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AtualizaImagensQuestoesUseCase : IAtualizaImagensQuestoesUseCase
    {

        private readonly IMediator mediator;

        public AtualizaImagensQuestoesUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var questoesSerap = await mediator.Send(new ObterQuestoesComImagensNaoSincronizadasQuery());

            foreach (var questaoSerap in questoesSerap)
            {
                questaoSerap.TrataArquivosTextoBase();
                if (questaoSerap.Arquivos.Any())
                {
                    await mediator.Send(new QuestaoParaAtualizarCommand(questaoSerap));

                    questaoSerap.Arquivos = await mediator.Send(new ObterTamanhoArquivosQuery(questaoSerap.Arquivos));

                    foreach (var arquivoParaPersistir in questaoSerap.Arquivos)
                    {
                        var arquivoId = await mediator.Send(new ArquivoPersistirCommand(arquivoParaPersistir));
                        await mediator.Send(new QuestaoArquivoPersistirCommand(questaoSerap.Id, arquivoId));
                    }

                }
            }
            return true;
        }
    }
}