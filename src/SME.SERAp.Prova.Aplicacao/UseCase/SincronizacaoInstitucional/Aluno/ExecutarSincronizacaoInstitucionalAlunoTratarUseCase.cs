using MediatR;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExecutarSincronizacaoInstitucionalAlunoTratarUseCase : AbstractUseCase, IExecutarSincronizacaoInstitucionalAlunoTratarUseCase
    {
        public ExecutarSincronizacaoInstitucionalAlunoTratarUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var aluno = mensagemRabbit.ObterObjetoMensagem<AlunoParaSincronizacaoInstitucionalDto>();
            
            if (aluno == null)
                throw new NegocioException("Não foi possível localizar o aluno para sincronizar.");

            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TratarAlunoDeficiencia, aluno.AlunoCodigo,
                mensagemRabbit.CodigoCorrelacao));

            return true;
        }
    }
}
