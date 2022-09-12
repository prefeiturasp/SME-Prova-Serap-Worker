using MediatR;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.UseCase.PersistenciaSerap
{
    public class TratarReaberturaProvaAlunoUseCase : ITratarReaberturaProvaAlunoUseCase
    {
        private readonly IMediator mediator;

        public TratarReaberturaProvaAlunoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            return true;
            //* delete from prova_aluno where aluno_ra and prova_id
            //* incluir o log de quem fez a solicitação
            // * chama fila do acompanhamento para atualizar o registro
            // Incluir log 
            // chamar a fila 
            //IncluirProvaAlunoReaberturaCommand
        }
    }
}
