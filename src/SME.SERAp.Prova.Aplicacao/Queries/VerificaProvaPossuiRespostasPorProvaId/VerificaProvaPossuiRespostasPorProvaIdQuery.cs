﻿using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class VerificaProvaPossuiRespostasPorProvaIdQuery : IRequest<bool>
    {
        public VerificaProvaPossuiRespostasPorProvaIdQuery(long provaId)
        {
            ProvaId = provaId;
        }

        public long ProvaId { get; set; }
    }
}
