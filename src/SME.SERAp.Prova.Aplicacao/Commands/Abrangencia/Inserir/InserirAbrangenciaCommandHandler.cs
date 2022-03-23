using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class InserirAbrangenciaCommandHandler : IRequestHandler<InserirAbrangenciaCommand, bool>
    {

        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public InserirAbrangenciaCommandHandler(IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new System.ArgumentNullException(nameof(repositorioAbrangencia));
        }

        public async Task<bool> Handle(InserirAbrangenciaCommand request, CancellationToken cancellationToken)
        {
            await repositorioAbrangencia.SalvarAsync(request.Abrangencia);
            return true;
        }
    }
}
