using MediatR;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUsuariosPorGrupoCoreSsoQuery : IRequest<IEnumerable<UsuarioCoreSsoDto>>
    {
        public ObterUsuariosPorGrupoCoreSsoQuery(Guid grupoId)
        {
            GrupoId = grupoId;
        }

        public Guid GrupoId { get; set; }
    }
}
