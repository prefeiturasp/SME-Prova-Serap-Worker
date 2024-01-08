using MediatR;
using SME.SERAp.Prova.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;
using SME.SERAp.Prova.Infra.Dtos.Tai;
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
                var cadernoProvaTaiTratar = mensagemRabbit.ObterObjetoMensagem<CadernoProvaTaiTratarDto>();

                if (cadernoProvaTaiTratar.ProvaId == 0)
                    throw new NegocioException("O Id da prova deve ser informado.");
                
                if (cadernoProvaTaiTratar.ProvaLegadoId == 0)
                    throw new NegocioException("O Id da prova do legado deve ser informado.");

                await PublicarFilaTratarCadernoAluno(cadernoProvaTaiTratar);

                return true;
            }
            catch (Exception ex)
            {
                throw new ErroException($"Erro ao tratar cadernos da prova TAI: {ex.Message}");
            }
        }

        private async Task PublicarFilaTratarCadernoAluno(CadernoProvaTaiTratarDto cadernoProvaTaiTratar)
        {
            const string caderno = "1";
            
            var alunosAtivosProvaTaiSemCaderno = cadernoProvaTaiTratar.AlunosProvaTaiSemCaderno
                .Where(a => a.Ativo());

            foreach (var item in alunosAtivosProvaTaiSemCaderno)
            {
                var msg = new AlunoCadernoProvaTaiTratarDto(item.ProvaId, item.AlunoId, item.ProvaLegadoId,
                    item.AlunoRa, cadernoProvaTaiTratar.Disciplina, cadernoProvaTaiTratar.ItensAmostra,
                    cadernoProvaTaiTratar.Ano, caderno);
                
                await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TratarCadernoAlunoProvaTai, msg));
            }
        }
    }
}
