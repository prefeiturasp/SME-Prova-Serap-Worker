using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUsuariosPorGrupoCoreSsoQueryHandler : IRequestHandler<ObterUsuariosPorGrupoCoreSsoQuery, IEnumerable<UsuarioCoreSsoDto>>
    {

        private readonly IRepositorioUsuarioCoreSso repositorioUsuarioCoreSso;

        public ObterUsuariosPorGrupoCoreSsoQueryHandler(IRepositorioUsuarioCoreSso repositorioUsuarioCoreSso)
        {
            this.repositorioUsuarioCoreSso = repositorioUsuarioCoreSso ?? throw new System.ArgumentNullException(nameof(repositorioUsuarioCoreSso));
        }

        public async Task<IEnumerable<UsuarioCoreSsoDto>> Handle(ObterUsuariosPorGrupoCoreSsoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioUsuarioCoreSso.ObterUsuariosPorGrupoCoreSso(request.GrupoId);
        }
    }
}
