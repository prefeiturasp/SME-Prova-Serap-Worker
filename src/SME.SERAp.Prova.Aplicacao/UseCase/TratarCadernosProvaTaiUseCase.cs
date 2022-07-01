using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarCadernosProvaTaiUseCase : ITratarCadernosProvaTaiUseCase
    {

        private readonly IMediator mediator;

        public TratarCadernosProvaTaiUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            try
            {
                var provaId = long.Parse(mensagemRabbit.Mensagem.ToString());
                var alunosProvaTaiSemCaderno = await mediator.Send(new ObterAlunosProvaTaiSemCadernoQuery(provaId));

                foreach (var item in alunosProvaTaiSemCaderno.Where(a => a.Ativo()))
                {
                    await PublicarFilaTratarCadernoAluno(item.ProvaId, item.AlunoId, item.ProvaLegadoId);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task PublicarFilaTratarCadernoAluno(long provaId, long alunoId, long provaLegadoId)
        {
            var msg = new AlunoCadernoProvaTaiTratarDto(provaId, alunoId, provaLegadoId);
            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TratarCadernoAlunoProvaTai, msg));
        }
    }
}
