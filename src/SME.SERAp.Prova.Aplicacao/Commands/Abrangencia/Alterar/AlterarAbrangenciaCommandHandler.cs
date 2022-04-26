using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.Commands.Abrangencia.Alterar
{
    public class AlterarAbrangenciaCommandHandler : IRequestHandler<AlterarAbrangenciaCommand, bool>
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public AlterarAbrangenciaCommandHandler(IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new System.ArgumentNullException(nameof(repositorioAbrangencia));
        }

        public async Task<bool> Handle(AlterarAbrangenciaCommand request, CancellationToken cancellationToken)
        {
            await repositorioAbrangencia.UpdateAsync(request.Abrangencia);
            return true;
        }
    }
}
