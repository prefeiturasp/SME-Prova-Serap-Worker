using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao.Queries
{
   public class ObterPermissoesProvaLegadoQuery : IRequest<IEnumerable<ProvaGrupoPermissaoDto>>
    {
        public ObterPermissoesProvaLegadoQuery(long provaLegadoId)
        {
            ProvaLegadoId = provaLegadoId;
        }

        public long ProvaLegadoId { get; set; }
    }
}

