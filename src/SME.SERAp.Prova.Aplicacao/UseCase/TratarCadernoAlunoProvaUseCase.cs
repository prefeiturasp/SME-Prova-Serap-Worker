using MediatR;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.UseCase
{
    public class TratarCadernoAlunoProvaUseCase : AbstractUseCase, ITratarCadernoAlunoProvaUseCase
    {
        public TratarCadernoAlunoProvaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var alunoProva = mensagemRabbit.ObterObjetoMensagem<AlunoCadernoProvaTaiTratarDto>();
            if(alunoProva == null)
                throw new NegocioException("Dados do aluno e prova não informados.");

            var existeCadernoAluno = await mediator.Send(new ExisteCadernoAlunoPorProvaIdAlunoIdQuery(alunoProva.ProvaId, alunoProva.AlunoId));
            if (existeCadernoAluno)
                throw new NegocioException($"Aluno {alunoProva.AlunoId} ja possui caderno para a prova {alunoProva.ProvaId}.");

            var cadernoAluno = new CadernoAluno(
                   alunoProva.AlunoId,
                   alunoProva.ProvaId,
                   alunoProva.Caderno);

            await mediator.Send(new CadernoAlunoIncluirCommand(cadernoAluno));

            return true;
        }
    }
}
