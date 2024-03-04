using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Aplicacao.Commands.ConsolidarProvaRespostaAdesaoManual
{
    public class ConsolidarProvaRespostaAdesaoManualCommand : IRequest<bool>
    {
        public ConsolidarProvaRespostaAdesaoManualCommand(long provaId)
        {
            ProvaId = provaId;
        }

        public long ProvaId { get; set; }
    }
}
