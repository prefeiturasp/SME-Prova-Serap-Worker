using MediatR;
using SME.SERAp.Prova.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AtualizaImagensAlternativasUseCase : IAtualizaImagensAlternativasUseCase
    {

        private readonly IMediator mediator;

        public AtualizaImagensAlternativasUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var alternativasSerap = await mediator.Send(new ObterAlternativasComImagensNaoSincronizadasQuery());

            foreach (var alternativaSerap in alternativasSerap)
            {
                alternativaSerap.TrataArquivos();
                if (alternativaSerap.Arquivos.Any())
                {
                    await mediator.Send(new AlternativaParaAtualizarCommand(alternativaSerap));

                    alternativaSerap.Arquivos = await mediator.Send(new ObterTamanhoArquivosQuery(alternativaSerap.Arquivos));

                    foreach (var arquivoParaPersistir in alternativaSerap.Arquivos)
                    {
                        var arquivoId = await mediator.Send(new ArquivoPersistirCommand(arquivoParaPersistir));
                        await mediator.Send(new AlternativaArquivoPersistirCommand(alternativaSerap.Id, arquivoId));
                    }

                }
            }
            return true;
        }
    }
}