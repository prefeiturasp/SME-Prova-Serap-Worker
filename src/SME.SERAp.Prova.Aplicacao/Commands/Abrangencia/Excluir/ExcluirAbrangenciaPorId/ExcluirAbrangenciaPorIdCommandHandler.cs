using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExcluirAbrangenciaPorIdCommandHandler : IRequestHandler<ExcluirAbrangenciaPorIdCommand, bool>
    {

        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public ExcluirAbrangenciaPorIdCommandHandler(IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new System.ArgumentNullException(nameof(repositorioAbrangencia));
        }

        public async Task<bool> Handle(ExcluirAbrangenciaPorIdCommand request, CancellationToken cancellationToken)
        {
            return await repositorioAbrangencia.ExcluirPorId(request.Id);
        }
    }
}
