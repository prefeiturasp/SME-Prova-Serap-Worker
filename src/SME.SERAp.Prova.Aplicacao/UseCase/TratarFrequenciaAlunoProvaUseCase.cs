using MediatR;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarFrequenciaAlunoProvaUseCase : ITratarFrequenciaAlunoProvaUseCase
    {
        private readonly IMediator mediator;

        public TratarFrequenciaAlunoProvaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var aluno = mensagemRabbit.ObterObjetoMensagem<ProvaAlunoReduzidaDto>();

            if (aluno == null)
                throw new NegocioException("Aluno não foi informado");

            var frequenciaAluno = await mediator.Send(new ObterFrequenciaAlunoSgpQuery(aluno.AlunoRa, aluno.FinalizadoEm));

            await mediator.Send(new AtualizarFrequenciaAlunoCommand(aluno.ProvaAlunoId, frequenciaAluno));

            return true;
        }
    }
}