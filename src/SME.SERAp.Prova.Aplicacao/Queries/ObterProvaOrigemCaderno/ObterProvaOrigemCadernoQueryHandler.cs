using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterProvaOrigemCadernoQueryHandler : IRequestHandler<ObterProvaOrigemCadernoQuery, long?>
    {
        private readonly IRepositorioProva repositorioProva;

        public ObterProvaOrigemCadernoQueryHandler(IRepositorioProva repositorioProva)
        {
            this.repositorioProva = repositorioProva;
        }

        public async Task<long?> Handle(ObterProvaOrigemCadernoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioProva.ObterProvaOrigemCadernoAsync(request.ProvaId);
        }
    }
}
