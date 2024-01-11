using MediatR;
using SME.SERAp.Prova.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarAlunoProvaProficienciaSyncUseCase : ITratarAlunoProvaProficienciaSyncUseCase
    {
        private readonly IMediator mediator;

        public TratarAlunoProvaProficienciaSyncUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var alunosProva = await mediator.Send(new ObterAlunosSemProficienciaQuery());
            
            if (!alunosProva.Any())
                return true;
            
            foreach (var alunoProva in alunosProva.Where(a => a.DisciplinaId != null))
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.AlunoProvaProficienciaTratar, alunoProva));

            return true;
        }
    }
}
