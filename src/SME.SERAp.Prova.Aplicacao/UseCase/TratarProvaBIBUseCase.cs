using MediatR;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProvaBIBUseCase : ITratarProvaBIBUseCase
    {
        private readonly IMediator mediator;
        private readonly IServicoLog servicoLog;

        public TratarProvaBIBUseCase(IMediator mediator, IServicoLog servicoLog)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.servicoLog = servicoLog ?? throw new ArgumentNullException(nameof(servicoLog));
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var provaCadernoAluno = mensagemRabbit.ObterObjetoMensagem<ProvaCadernoAlunoDto>();
                var caderno = await mediator.Send(new ObterCadernoAlunoPorProvaIdAlunoIdQuery(provaCadernoAluno.ProvaId, provaCadernoAluno.AlunoId));

                77577775777757777577if (caderno == null)
                {
                    Random sortear = new Random();
                    var cadernoSorteado = sortear.Next(1, provaCadernoAluno.TotalCadernos + 1).ToString();
                    await mediator.Send(new CadernoAlunoIncluirCommand(new Dominio.CadernoAluno(provaCadernoAluno.AlunoId, provaCadernoAluno.ProvaId, cadernoSorteado)));
                }
            }
            catch (Exception ex)
            {
                servicoLog.Registrar("$Erro ao incluir caderno para o aluno", ex);
            }
            return true;
        }
    }
}