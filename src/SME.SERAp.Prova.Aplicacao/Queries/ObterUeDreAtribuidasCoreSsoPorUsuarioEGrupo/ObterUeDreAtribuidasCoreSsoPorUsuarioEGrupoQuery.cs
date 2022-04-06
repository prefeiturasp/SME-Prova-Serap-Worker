using System;
using System.Collections.Generic;
using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUeDreAtribuidasCoreSsoPorUsuarioEGrupoQuery : IRequest<IEnumerable<string>>
    {
        public ObterUeDreAtribuidasCoreSsoPorUsuarioEGrupoQuery(Guid usuarioIdCoreSso, Guid grupoIdCoreSso)
        {
            UsuarioIdCoreSso = usuarioIdCoreSso;
            GrupoIdCoreSso = grupoIdCoreSso;
        }

        public Guid UsuarioIdCoreSso { get; set; }
        public Guid GrupoIdCoreSso { get; set; }
    }
}
