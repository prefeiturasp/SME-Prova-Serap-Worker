using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUsuarioSerapCoreSsoPorIdCoreSsoQueryHandler : IRequestHandler<ObterUsuarioSerapCoreSsoPorIdCoreSsoQuery, UsuarioSerapCoreSso>
    {
        private readonly IRepositorioUsuarioSerapCoreSso repositorioUsuarioSerapCoreSso;

        public ObterUsuarioSerapCoreSsoPorIdCoreSsoQueryHandler(IRepositorioUsuarioSerapCoreSso repositorioUsuarioSerapCoreSso)
        {
            this.repositorioUsuarioSerapCoreSso = repositorioUsuarioSerapCoreSso ?? throw new System.ArgumentNullException(nameof(repositorioUsuarioSerapCoreSso));
        }

        public async Task<UsuarioSerapCoreSso> Handle(ObterUsuarioSerapCoreSsoPorIdCoreSsoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioUsuarioSerapCoreSso.ObterPorIdCoreSso(request.IdCoreSso);
        }
    }
}
