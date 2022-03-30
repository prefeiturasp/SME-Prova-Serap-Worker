using MediatR;
using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAbrangenciaPorGrupoIdQuery : IRequest<IEnumerable<Abrangencia>>
    {
        public ObterAbrangenciaPorGrupoIdQuery(long grupoId)
        {
            GrupoId = grupoId;
        }

        public long GrupoId { get; set; }
    }
}
