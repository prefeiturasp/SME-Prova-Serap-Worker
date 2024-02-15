using System.Collections.Generic;
using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterQuestoesCompletasPorProvaIdParaCacheQuery : IRequest<IEnumerable<QuestaoCompleta>>
    {
        public ObterQuestoesCompletasPorProvaIdParaCacheQuery(long provaId)
        {
            ProvaId = provaId;
        }

        public long ProvaId { get; }
    }
}