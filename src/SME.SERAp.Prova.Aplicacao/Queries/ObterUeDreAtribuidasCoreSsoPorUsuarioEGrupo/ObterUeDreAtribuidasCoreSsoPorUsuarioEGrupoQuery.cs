using System;
using System.Collections.Generic;
using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUeDreAtribuidasCoreSsoPorUsuarioEGrupoQuery : IRequest<IEnumerable<string>>
    {
        public ObterUeDreAtribuidasCoreSsoPorUsuarioEGrupoQuery(Guid usuarioIdCoreSso, Guid grupoIdCoreSso, string codigoRf)
        {
            UsuarioIdCoreSso = usuarioIdCoreSso;
            GrupoIdCoreSso = grupoIdCoreSso;
            CodigoRf = codigoRf;
        }

        public Guid UsuarioIdCoreSso { get; }
        public Guid GrupoIdCoreSso { get; }
        public string CodigoRf { get; }
    }
}
