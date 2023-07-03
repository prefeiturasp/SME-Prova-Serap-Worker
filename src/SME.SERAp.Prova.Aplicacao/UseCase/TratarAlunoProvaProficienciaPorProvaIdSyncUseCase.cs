using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarAlunoProvaProficienciaPorProvaIdSyncUseCase : ITratarAlunoProvaProficienciaPorProvaIdSyncUseCase
    {
        private readonly IMediator mediator;

        public TratarAlunoProvaProficienciaPorProvaIdSyncUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var mensagem = mensagemRabbit.Mensagem;

            if (mensagem == null)
                return true;
            
            var provaId = long.Parse(mensagem.ToString() ?? string.Empty);

            var alunosProva = (await mediator.Send(new ObterAlunosSemProficienciaPorProvaIdQuery(provaId))).ToList();

            if (!alunosProva.Any())
                return true;
            
            foreach (var alunoProva in alunosProva.Where(a => a.DisciplinaId != null))
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.AlunoProvaProficienciaTratar, alunoProva));

            return true;            
        }
    }
}