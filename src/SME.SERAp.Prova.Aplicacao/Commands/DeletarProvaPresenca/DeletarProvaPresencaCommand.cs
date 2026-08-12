using MediatR;

namespace SME.SERAp.Prova.Aplicacao.Commands.DeletarProvaPresenca
{
    public class DeletarProvaPresencaCommand : IRequest<bool>
    {
        public DeletarProvaPresencaCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }
}