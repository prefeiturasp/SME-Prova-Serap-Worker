using MediatR;
using SME.SERAp.Prova.Infra;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProvaLegadoUseCase : ITratarProvaLegadoUseCase
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
            var provaParaTratar = new Dominio.Prova(0, provaLegado.Descricao, provaLegado.Inicio, provaLegado.Fim, provaLegado.TotalItens, provaLegado.Id, provaLegado.TempoExecucao);

            if (provaAtual == null)
            {
                provaAtual = new Dominio.Prova();
                provaAtual.Id = await mediator.Send(new ProvaIncluirCommand(provaParaTratar));

            }
            else
            {
                provaParaTratar.Id = provaAtual.Id;

                await RemoverEntidadesFilhas(provaAtual);
                await mediator.Send(new ProvaAtualizarCommand(provaParaTratar));
            }

            foreach (var ano in provaLegado.Anos)
            {
                await mediator.Send(new ProvaAnoIncluirCommand(new Dominio.ProvaAno(ano, provaAtual.Id)));
            }
            
            await mediator.Send(
                new PublicaFilaRabbitCommand(RotasRabbit.QuestaoSync, provaLegado.Id));

            return true;
        }

        private async Task RemoverEntidadesFilhas(Dominio.Prova provaAtual)
        {
            await mediator.Send(new ProvaRemoverAnosPorIdCommand(provaAtual.Id));
            await mediator.Send(new ProvaRemoverAlternativasPorIdCommand(provaAtual.Id));
            await mediator.Send(new ProvaRemoverQuestoesPorIdCommand(provaAtual.Id));
        }
    }
}