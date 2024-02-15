using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterQuestoesCompletasPorProvaIdParaCacheQueryHandler : IRequestHandler<ObterQuestoesCompletasPorProvaIdParaCacheQuery, IEnumerable<QuestaoCompleta>>
    {
        private readonly IRepositorioQuestaoCompleta repositorioQuestaoCompleta;

        public ObterQuestoesCompletasPorProvaIdParaCacheQueryHandler(IRepositorioQuestaoCompleta repositorioQuestaoCompleta)
        {
            this.repositorioQuestaoCompleta = repositorioQuestaoCompleta ?? throw new ArgumentNullException(nameof(repositorioQuestaoCompleta));
        }

        public async Task<IEnumerable<QuestaoCompleta>> Handle(ObterQuestoesCompletasPorProvaIdParaCacheQuery request, CancellationToken cancellationToken)
        {
            return await repositorioQuestaoCompleta.ObterQuestoesCompletasPorProvaIdParaCacheAsync(request.ProvaId);
        }
    }
}