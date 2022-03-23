using System.Collections.Generic;
using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUeDreAtribuidasCoreSsoPorUsuarioEGrupoQueryHandler : IRequestHandler<ObterUeDreAtribuidasCoreSsoPorUsuarioEGrupoQuery, IEnumerable<string>>
    {

        private readonly IRepositorioGeralCoreSso repositorioGeralCoreSso;

        public ObterUeDreAtribuidasCoreSsoPorUsuarioEGrupoQueryHandler(IRepositorioGeralCoreSso repositorioGeralCoreSso)
        {
            this.repositorioGeralCoreSso = repositorioGeralCoreSso ?? throw new System.ArgumentNullException(nameof(repositorioGeralCoreSso));
        }

        public async Task<IEnumerable<string>> Handle(ObterUeDreAtribuidasCoreSsoPorUsuarioEGrupoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioGeralCoreSso.ObterUeDreAtribuidasCoreSso(request.UsuarioIdCoreSso, request.GrupoIdCoreSso);
        }
    }
}
