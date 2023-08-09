﻿using MediatR;
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
                
                if (string.IsNullOrEmpty(cadernoProvaTaiTratar.Disciplina))
                    throw new NegocioException("A disciplina da prova deve ser informado.");                    
                
                var alunosProvaTaiSemCaderno = await mediator.Send(new ObterAlunosProvaTaiSemCadernoQuery(cadernoProvaTaiTratar.ProvaId));
                
                if (alunosProvaTaiSemCaderno == null)
                    throw new NegocioException("Todos os alunos já possuem cadernos para a prova.");

                foreach (var item in alunosProvaTaiSemCaderno.Where(a => a.Ativo()))
                    await PublicarFilaTratarCadernoAluno(item.ProvaId, item.AlunoId, item.ProvaLegadoId, item.AlunoRa, cadernoProvaTaiTratar.Disciplina);
                
                return true;
            }
            catch (Exception ex)
            {
                throw new ErroException($"Erro ao tratar cadernos da prova TAI: {ex.Message}");
            }
        }

        private async Task PublicarFilaTratarCadernoAluno(long provaId, long alunoId, long provaLegadoId, long alunoRa, string disciplina)
        {
            var msg = new AlunoCadernoProvaTaiTratarDto(provaId, alunoId, provaLegadoId, alunoRa, disciplina);
            await mediator.Send(new PublicaFilaRabbitCommand(RotasRabbit.TratarCadernoAlunoProvaTai, msg));
        }
    }
}
