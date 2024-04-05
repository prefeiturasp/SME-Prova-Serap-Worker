using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterQuestoesAtualizadasQuery : IRequest<IEnumerable<QuestaoAtualizada>>
    {
        public ObterQuestoesAtualizadasQuery(long provaId, int pagina, int quantidade)
        {
            ProvaId = provaId;
            Pagina = pagina;
            Quantidade = quantidade;
        }

        public long ProvaId { get; set; }
        public int Pagina { get; set; }
        public int Quantidade { get; set; }
    }
}
