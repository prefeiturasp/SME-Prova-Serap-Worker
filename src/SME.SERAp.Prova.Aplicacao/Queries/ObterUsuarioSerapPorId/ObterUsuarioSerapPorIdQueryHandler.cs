using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUsuarioSerapPorIdQueryHandler : IRequestHandler<ObterUsuarioSerapPorIdQuery, UsuarioSerapCoreSso>
    {

        private readonly IRepositorioUsuarioSerapCoreSso repositorioUsuarioSerapCoreSso;

        public ObterUsuarioSerapPorIdQueryHandler(IRepositorioUsuarioSerapCoreSso repositorioUsuarioSerapCoreSso)
        {
            this.repositorioUsuarioSerapCoreSso = repositorioUsuarioSerapCoreSso ?? throw new System.ArgumentNullException(nameof(repositorioUsuarioSerapCoreSso));
        }

        public async Task<UsuarioSerapCoreSso> Handle(ObterUsuarioSerapPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioUsuarioSerapCoreSso.ObterPorIdAsync(request.Id);
        }

    }
}
