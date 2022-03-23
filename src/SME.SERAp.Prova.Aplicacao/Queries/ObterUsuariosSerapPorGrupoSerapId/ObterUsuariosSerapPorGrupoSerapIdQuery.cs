using System.Collections.Generic;
using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUsuariosSerapPorGrupoSerapIdQuery : IRequest<IEnumerable<UsuarioSerapCoreSso>>
    {
        public ObterUsuariosSerapPorGrupoSerapIdQuery(long grupoSerapId)
        {
            GrupoSerapId = grupoSerapId;
        }

        public long GrupoSerapId { get; set; }
    }
}
