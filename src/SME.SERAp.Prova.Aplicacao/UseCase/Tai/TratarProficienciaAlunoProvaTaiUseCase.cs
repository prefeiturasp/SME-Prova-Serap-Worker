using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TratarProficienciaAlunoProvaTaiUseCase : AbstractUseCase, ITratarProficienciaAlunoProvaTaiUseCase
    {
        public TratarProficienciaAlunoProvaTaiUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var proficienciaAlunoProvaTai = mensagemRabbit.ObterObjetoMensagem<ProficienciaAlunoProvaTaiDto>();
            if (proficienciaAlunoProvaTai == null)
                throw new NegocioException($"É preciso informar os dados de proficiencia.");

            await Validacoes(proficienciaAlunoProvaTai);

            return default;
        }

        private async Task Validacoes(ProficienciaAlunoProvaTaiDto proficienciaAlunoProvaTai)
        {
            var prova = await mediator.Send(new ObterProvaPorIdQuery(proficienciaAlunoProvaTai.ProvaId));
            if (prova == null)
                throw new NegocioException($"Prova não encontrada.");

            if (!prova.FormatoTai)
                throw new NegocioException($"Prova não é formato TAI.");
        }
    }
}
