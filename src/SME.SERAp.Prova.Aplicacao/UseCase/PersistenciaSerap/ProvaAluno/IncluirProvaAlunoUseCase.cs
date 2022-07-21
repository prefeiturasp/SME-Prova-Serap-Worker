using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class IncluirProvaAlunoUseCase : AbstractUseCase, IIncluirProvaAlunoUseCase
    {
        public IncluirProvaAlunoUseCase(IMediator mediator) : base(mediator)
        {
        }
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var provaAluno = mensagemRabbit.ObterObjetoMensagem<ProvaAluno>();
            return await mediator.Send(new IncluirProvaAlunoCommand(provaAluno));
        }
    }
}