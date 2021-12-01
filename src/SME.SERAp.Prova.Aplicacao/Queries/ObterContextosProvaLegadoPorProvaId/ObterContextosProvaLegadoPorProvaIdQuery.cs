﻿using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterContextosProvaLegadoPorProvaIdQuery : IRequest<IEnumerable<ContextoProvaLegadoDto>>
    {
        public ObterContextosProvaLegadoPorProvaIdQuery(long provaId)
        {
            ProvaId = provaId;
        }

        public long ProvaId { get; set; }
    }
}
