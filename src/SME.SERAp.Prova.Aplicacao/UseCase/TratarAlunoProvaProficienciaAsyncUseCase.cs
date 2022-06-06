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
            var alunosProvas = await mediator.Send(new ObterAlunosSemProficienciaQuery());
            if (alunosProvas.Any())
            {
                foreach (var alunoProva in alunosProvas)
                {
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.AlunoProvaProficienciaTratar, alunoProva));
                }
            }

            return true;
        }
    }
}
