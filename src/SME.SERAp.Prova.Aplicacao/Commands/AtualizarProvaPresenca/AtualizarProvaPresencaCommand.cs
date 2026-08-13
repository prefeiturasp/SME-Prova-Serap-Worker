using MediatR;
using SME.SERAp.Prova.Infra.Dtos.Presenca;

namespace SME.SERAp.Prova.Aplicacao.Commands.AtualizarProvaPresenca
{
    public class AtualizarProvaPresencaCommand : IRequest<bool>
    {
        public AtualizarProvaPresencaCommand(AtualizarProvaPresencaDto provaPresencaDto)
        {
            ProvaPresencaDto = provaPresencaDto;
        }

        public AtualizarProvaPresencaDto ProvaPresencaDto { get; set; }
    }
}