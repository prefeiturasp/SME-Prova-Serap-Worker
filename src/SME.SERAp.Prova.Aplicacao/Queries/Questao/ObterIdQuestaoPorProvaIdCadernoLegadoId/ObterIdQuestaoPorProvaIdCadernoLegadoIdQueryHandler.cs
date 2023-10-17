using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterIdQuestaoPorProvaIdCadernoLegadoIdQueryHandler : IRequestHandler<ObterIdQuestaoPorProvaIdCadernoLegadoIdQuery, long>
    {
        private readonly IRepositorioQuestao repositorioQuestao;

        public ObterIdQuestaoPorProvaIdCadernoLegadoIdQueryHandler(IRepositorioQuestao repositorioQuestao)
        {
            this.repositorioQuestao = repositorioQuestao ?? throw new ArgumentNullException(nameof(repositorioQuestao));
        }

        public async Task<long> Handle(ObterIdQuestaoPorProvaIdCadernoLegadoIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioQuestao.ObterIdQuestaoPorProvaIdCadernoLegadoId(request.ProvaId, request.Caderno, request.QuestaoLegadoId);
        }
    }
}
