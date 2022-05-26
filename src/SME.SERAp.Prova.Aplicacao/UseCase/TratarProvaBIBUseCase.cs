using MediatR;
using Sentry;
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
            try
            {
                var provaCadernoAluno = mensagemRabbit.ObterObjetoMensagem<ProvaCadernoAlunoDto>();
                Random sortear = new Random();
                var cadernoSorteado = sortear.Next(1, provaCadernoAluno.TotalCadernos).ToString();

                await mediator.Send(new CadernoAlunoIncluirCommand(new Dominio.CadernoAluno(provaCadernoAluno.AlunoId, provaCadernoAluno.ProvaId, cadernoSorteado)));
            }
            catch (Exception ex)
            {
                SentrySdk.AddBreadcrumb($"Erro ao incluir caderno para o aluno");
                SentrySdk.CaptureException(ex);
            }
            return true;
        }
    }
}