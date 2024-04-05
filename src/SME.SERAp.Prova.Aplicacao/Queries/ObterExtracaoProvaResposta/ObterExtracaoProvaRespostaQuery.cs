using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterExtracaoProvaRespostaQuery : IRequest<IEnumerable<ConsolidadoProvaRespostaDto>>
    {
        public long ProvaSerapId { get; }

        public string DreCodigoEol { get; }

        public string UeCodigoEol { get; }

        public string[] TurmasCodigosEol { get;  }

        public ObterExtracaoProvaRespostaQuery(long provaSerapId, string dreCodigoEol, string ueCodigoEol, string[] turmasCodigosEol)
        {
            ProvaSerapId = provaSerapId;
            DreCodigoEol = dreCodigoEol;
            UeCodigoEol = ueCodigoEol;
            TurmasCodigosEol = turmasCodigosEol;
        }

        public ObterExtracaoProvaRespostaQuery()
        {
        }
    }
}