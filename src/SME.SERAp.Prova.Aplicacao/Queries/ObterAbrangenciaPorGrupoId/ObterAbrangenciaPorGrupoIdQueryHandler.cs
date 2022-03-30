using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAbrangenciaPorGrupoIdQueryHandler : IRequestHandler<ObterAbrangenciaPorGrupoIdQuery, IEnumerable<Abrangencia>>
    {

        private readonly IRepositorioAbrangencia repositorioAbrangencia;

        public ObterAbrangenciaPorGrupoIdQueryHandler(IRepositorioAbrangencia repositorioAbrangencia)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new System.ArgumentNullException(nameof(repositorioAbrangencia));
        }

        public async Task<IEnumerable<Abrangencia>> Handle(ObterAbrangenciaPorGrupoIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAbrangencia.ObterPorGrupoId(request.GrupoId);
        }
    }
}
