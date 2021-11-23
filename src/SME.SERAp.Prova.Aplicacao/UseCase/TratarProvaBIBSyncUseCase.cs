using MediatR;
using SME.SERAp.Prova.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProvaBIBSyncUseCase : ITratarProvaBIBSyncUseCase
    {
        private readonly IMediator mediator;

        public TratarProvaBIBSyncUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var provaBIB = mensagemRabbit.ObterObjetoMensagem<ProvaBIBSyncDto>();

            var turmas = await mediator.Send(new ObterTurmasPorAnoEAnoLetivoQuery(provaBIB.Ano, DateTime.Now.Year));

            if(turmas.Any())
            {
                foreach(var turma in turmas)
                {
                    await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.ProvaBIBTratar, 
                        new ProvaTurmaBIBSyncDto(provaBIB.ProvaId, provaBIB.TotalCadernos, turma.Id)));
                }
            }
            
            return true;
        }
    }
}