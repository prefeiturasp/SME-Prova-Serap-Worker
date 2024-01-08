using MediatR;
using SME.SERAp.Prova.Infra;
using System;
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
            var provaCadernoAluno = mensagemRabbit.ObterObjetoMensagem<ProvaCadernoAlunoDto>();
            var caderno = await mediator.Send(new ObterCadernoAlunoPorProvaIdAlunoIdQuery(provaCadernoAluno.ProvaId, provaCadernoAluno.AlunoId));

            if (caderno == null)
            {
                string cadernoSorteado;

                var provaOrigem = await mediator.Send(new ObterProvaOrigemCadernoQuery(provaCadernoAluno.ProvaId));
                if (provaOrigem.HasValue)
                {
                    caderno = await mediator.Send(new ObterCadernoAlunoPorProvaIdAlunoIdQuery(provaOrigem.Value, provaCadernoAluno.AlunoId));
                    if (caderno == null)
                        throw new Exception($"Prova {provaCadernoAluno.ProvaId} Caderno não encontrado para o aluno { provaCadernoAluno.AlunoId} na prova de origem {provaOrigem.Value}");

                    cadernoSorteado = caderno.Caderno;
                }
                else
                {
                    Random sortear = new Random();
                    cadernoSorteado = sortear.Next(1, provaCadernoAluno.TotalCadernos + 1).ToString();
                }

                await mediator.Send(new CadernoAlunoIncluirCommand(new Dominio.CadernoAluno(provaCadernoAluno.AlunoId, provaCadernoAluno.ProvaId, cadernoSorteado)));
            }

            return true;
        }
    }
}