using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Aplicacao.Queries
{
    public class ObterProvaGruposPermissaoQuery : IRequest<IEnumerable<Dominio.ProvaGrupoPermissao>>
    {
        public ObterProvaGruposPermissaoQuery(long provaId)
        {
            ProvaId = provaId;
        }

        public long ProvaId { get; set; }
    }
}