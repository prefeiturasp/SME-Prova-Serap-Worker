﻿using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterExtracaoProvaRespostaQuery : IRequest<IEnumerable<ConsolidadoProvaRespostaDto>>
    {
        public long ProvaSerapId { get; set; }

        public ObterExtracaoProvaRespostaQuery(long provaSerapId)
        {
            ProvaSerapId = provaSerapId;
        }
    }
}