using MediatR;
using SME.SERAp.Prova.Infra;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProvaLegadoUseCase : ITratarProvaLegadoLegadoUseCase
    {
        private readonly IMediator mediator;

        public TratarProvaLegadoUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var provaId = long.Parse(mensagemRabbit.Mensagem.ToString());

            var provaLegado = await mediator.Send(new ObterProvaLegadoDetalhesPorIdQuery(provaId));

            if (provaLegado == null)
                throw new System.Exception($"Prova {provaLegado} não localizada!");

            var provaAtual = await mediator.Send(new ObterProvaDetalhesPorIdQuery(provaLegado.Id));
            var provaParaTratar = new Dominio.Prova(0, provaLegado.Descricao, provaLegado.Inicio, provaLegado.Fim, provaLegado.TotalItens, provaLegado.Id);

            if (provaAtual == null)
            {
                provaAtual = new Dominio.Prova();
                provaAtual.Id = await mediator.Send(new ProvaIncluirCommand(provaParaTratar));

            }
            else
            {
                provaParaTratar.Id = provaAtual.Id;
                await mediator.Send(new ProvaAtualizarCommand(provaParaTratar));

                await mediator.Send(new ProvaRemoverAnosCommand(provaAtual.Id));
            }

            foreach (var ano in provaLegado.Anos)
            {
                await mediator.Send(new ProvaAnoIncluirCommand(new Dominio.ProvaAno(ano, provaAtual.Id)));
            }

            return true;
        }
    }
}