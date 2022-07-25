using MediatR;
using System;

namespace SME.SERAp.Prova.Aplicacao
{
    public class QuestaoCompletaIncluirCommand : IRequest<bool>
    {
        public QuestaoCompletaIncluirCommand(long id, long questaoLegadoId, string json, DateTime ultimaAtualizacao)
        {
            Id = id;
            QuestaoLegadoId = questaoLegadoId;
            Json = json;
            UltimaAtualizacao = ultimaAtualizacao;
        }

        public long Id { get; set; }
        public long QuestaoLegadoId { get; set; }
        public string Json { get; set; }
        public DateTime UltimaAtualizacao { get; set; }
    }
}
