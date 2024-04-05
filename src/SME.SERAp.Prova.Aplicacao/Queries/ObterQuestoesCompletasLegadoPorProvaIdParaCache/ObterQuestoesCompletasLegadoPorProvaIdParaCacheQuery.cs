using System.Collections.Generic;
using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterQuestoesCompletasLegadoPorProvaIdParaCacheQuery : IRequest<IEnumerable<QuestaoCompleta>>
    {
        public ObterQuestoesCompletasLegadoPorProvaIdParaCacheQuery(long provaId)
        {
            ProvaId = provaId;
        }

        public long ProvaId { get; }
    }
}