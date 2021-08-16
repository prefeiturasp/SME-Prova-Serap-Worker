using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ProvaAnoIncluirCommandHandler : IRequestHandler<ProvaAnoIncluirCommand, long>
    {
        private readonly IRepositorioProvaAno repositorioProvaAno;

        public ProvaAnoIncluirCommandHandler(IRepositorioProvaAno repositorioProvaAno)
        {
            this.repositorioProvaAno = repositorioProvaAno ?? throw new System.ArgumentNullException(nameof(repositorioProvaAno));
        }
        public async Task<long> Handle(ProvaAnoIncluirCommand request, CancellationToken cancellationToken)
        {
            return await repositorioProvaAno.IncluirAsync(request.ProvaAno);
        }
    }
}
