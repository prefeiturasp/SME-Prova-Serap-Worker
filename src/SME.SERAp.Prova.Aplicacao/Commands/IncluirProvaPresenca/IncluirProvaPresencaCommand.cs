using MediatR;
using SME.SERAp.Prova.Infra.Dtos.Presenca;

namespace SME.SERAp.Prova.Aplicacao.Commands.IncluirProvaPresenca
{
    public class IncluirProvaPresencaCommand : IRequest<long>
    {
        public IncluirProvaPresencaCommand(CriarProvaPresencaDto provaPresencaDto)
        {
            ProvaPresencaDto = provaPresencaDto;
        }

        public CriarProvaPresencaDto ProvaPresencaDto { get; set; }
    }
}