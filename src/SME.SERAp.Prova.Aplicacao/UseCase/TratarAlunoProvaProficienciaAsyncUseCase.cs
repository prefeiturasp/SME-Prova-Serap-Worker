using MediatR;
using SME.SERAp.Prova.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarAlunoProvaProficienciaAsyncUseCase : ITratarAlunoProvaProficienciaAsyncUseCase
    {
        private readonly IMediator mediator;

        public TratarAlunoProvaProficienciaAsyncUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var provaId = long.Parse(mensagemRabbit.Mensagem.ToString());

            var alunosProva = await mediator.Send(new ObterAlunosSemProficienciaQuery(provaId));
            if (alunosProva.Any())
            {
                foreach (var alunoProva in alunosProva)
                {
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.AlunoProvaProficienciaTratar, alunoProva));
                }
            }

            return true;
        }
    }
}
