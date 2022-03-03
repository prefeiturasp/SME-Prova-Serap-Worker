using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAudiosPorQuestaoLegadoIdQueryHandler : IRequestHandler<ObterAudiosPorQuestaoLegadoIdQuery, IEnumerable<Arquivo>>
    {
        private readonly IRepositorioProvaLegado repositorioProvaLegado;
        public ObterAudiosPorQuestaoLegadoIdQueryHandler(IRepositorioProvaLegado repositorioProvaLegado)
        {
            this.repositorioProvaLegado = repositorioProvaLegado ??
                                          throw new ArgumentNullException(nameof(repositorioProvaLegado));
        }

        public async Task<IEnumerable<Arquivo>> Handle(ObterAudiosPorQuestaoLegadoIdQuery request,
            CancellationToken cancellationToken)
            => await repositorioProvaLegado.ObterAudiosPorQuestaoId(request.QuestaoId);
    }
}
