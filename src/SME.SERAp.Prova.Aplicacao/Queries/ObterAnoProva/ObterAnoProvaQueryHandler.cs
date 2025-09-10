using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.Queries.ObterAnoProva
{
    public class ObterAnoProvaQueryHandler : IRequestHandler<ObterAnoProvaQuery, int?>
    {
        private readonly IRepositorioProva repositorio;
        public ObterAnoProvaQueryHandler(IRepositorioProva repositorio)
        {
            this.repositorio = repositorio;
        }

        public Task<int?> Handle(ObterAnoProvaQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterAnoProva(request.ProvaId);
        }
    }
}
