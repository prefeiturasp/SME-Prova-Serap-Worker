using MediatR;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarFrequenciaAlunoProvaSyncUseCase : ITratarFrequenciaAlunoProvaSyncUseCase
    {
        private readonly IMediator mediator;

        public TratarFrequenciaAlunoProvaSyncUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {

            var alunos = await mediator.Send(new ObterProvaAlunoFinalizadasQuery());

            if(alunos.Any())
            {
                foreach(var aluno in alunos)
                {
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.AtualizarFrequenciaAlunoProvaTratar, aluno));
                }
            }
            

            return true;
        }
    }
}