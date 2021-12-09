using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AlterarTurmasCommandHandler : IRequestHandler<AlterarTurmasCommand, bool>
    {
        private readonly IRepositorioTurmaEntity repositorioTurmaEntity;

        public AlterarTurmasCommandHandler(IRepositorioTurmaEntity repositorioTurmaEntity)
        {
            this.repositorioTurmaEntity = repositorioTurmaEntity ?? throw new System.ArgumentNullException(nameof(repositorioTurmaEntity));
        }

        public async Task<bool> Handle(AlterarTurmasCommand request, CancellationToken cancellationToken)
        {
            await repositorioTurmaEntity.AlterarVariosAsync(request.Turmas);
            return true;
        }

    }
}
