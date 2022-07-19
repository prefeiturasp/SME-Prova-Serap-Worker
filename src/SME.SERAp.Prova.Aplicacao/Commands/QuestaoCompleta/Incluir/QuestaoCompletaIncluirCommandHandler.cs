using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class QuestaoCompletaIncluirCommandHandler : IRequestHandler<QuestaoCompletaIncluirCommand, bool>
    {
        private readonly IRepositorioQuestaoCompleta repositorioQuestaoCompleta;

        public QuestaoCompletaIncluirCommandHandler(IRepositorioQuestaoCompleta repositorioQuestaoCompleta)
        {
            this.repositorioQuestaoCompleta = repositorioQuestaoCompleta ?? throw new ArgumentException(nameof(repositorioQuestaoCompleta));
        }

        public async Task<bool> Handle(QuestaoCompletaIncluirCommand request, CancellationToken cancellationToken)
        {
            await repositorioQuestaoCompleta.IncluirOuUpdateAsync(new Dominio.QuestaoCompleta(request.Id, request.QuestaoLegadoId, request.Json, request.UltimaAtualizacao));
            return true;
        }
    }
}
