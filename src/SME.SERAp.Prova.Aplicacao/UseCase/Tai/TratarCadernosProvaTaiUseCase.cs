using MediatR;
using SME.SERAp.Prova.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;
using SME.SERAp.Prova.Infra.Exceptions;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarCadernosProvaTaiUseCase : AbstractUseCase, ITratarCadernosProvaTaiUseCase
    {
        public TratarCadernosProvaTaiUseCase(IMediator mediator) : base(mediator)
        {
        }        
        
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var provaId = long.Parse(mensagemRabbit.Mensagem.ToString() ?? string.Empty);

                if (provaId == 0)
                    throw new NegocioException("O Id da prova deve ser informado.");
                
                var alunosProvaTaiSemCaderno = await mediator.Send(new ObterAlunosProvaTaiSemCadernoQuery(provaId));

                foreach (var item in alunosProvaTaiSemCaderno.Where(a => a.Ativo()))
                    await PublicarFilaTratarCadernoAluno(item.ProvaId, item.AlunoId, item.ProvaLegadoId, item.AlunoRa);
                
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task PublicarFilaTratarCadernoAluno(long provaId, long alunoId, long provaLegadoId, long alunoRa)
        {
            var msg = new AlunoCadernoProvaTaiTratarDto(provaId, alunoId, provaLegadoId, alunoRa);
            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TratarCadernoAlunoProvaTai, msg));
        }
    }
}
