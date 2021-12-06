using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class InserirTurmasCommandHandler : IRequestHandler<InserirTurmasCommand, bool>
    {
        private readonly IRepositorioTurmaEntity repositorioTurmaEntity;

        public InserirTurmasCommandHandler(IRepositorioTurmaEntity repositorioTurmaEntity)
        {
            this.repositorioTurmaEntity = repositorioTurmaEntity ?? throw new System.ArgumentNullException(nameof(repositorioTurmaEntity));
        }

        public async Task<bool> Handle(InserirTurmasCommand request, CancellationToken cancellationToken)
        {
            await repositorioTurmaEntity.InserirVariosAsync(request.Turmas);
            return true;
        }

    }
}
