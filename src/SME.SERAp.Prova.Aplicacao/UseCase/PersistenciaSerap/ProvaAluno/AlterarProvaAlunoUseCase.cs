using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
  public  class AlterarProvaAlunoUseCase : AbstractUseCase, IAlterarProvaAlunoUseCase
    {
        public AlterarProvaAlunoUseCase(IMediator mediator) : base(mediator)
        {
        }
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var provaAluno = mensagemRabbit.ObterObjetoMensagem<ProvaAluno>();
            return await mediator.Send(new AtualizarProvaAlunoCommand(provaAluno));
        }
    }
}
