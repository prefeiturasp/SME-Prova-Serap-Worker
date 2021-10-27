using MediatR;
using SME.SERAp.Prova.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProvaBIBUseCase : ITratarProvaBIBUseCase
    {
        private readonly IMediator mediator;

        public TratarProvaBIBUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var turmaBIB = mensagemRabbit.ObterObjetoMensagem<ProvaTurmaBIBSyncDto>();

            var alunos = await mediator.Send(new ObterAlunosPorTurmaIdQuery(turmaBIB.TurmaId));

            var caderno = 1;
            if (alunos.Any())
            {
                foreach (var aluno in alunos)
                {
                    await mediator.Send(new CadernoAlunoIncluirCommand(new Dominio.CadernoAluno(aluno.Id, turmaBIB.ProvaId, caderno.ToString())));

                    caderno += 1;

                    if (caderno > turmaBIB.TotalCadernos)
                        caderno = 1;
                }
            }

            return true;
        }
    }
}