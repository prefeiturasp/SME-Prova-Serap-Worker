using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class VerificaAderirTodosProvaLegadoQuery : IRequest<bool>
    {
        public VerificaAderirTodosProvaLegadoQuery(long provaLegadoId)
        {
            ProvaLegadoId = provaLegadoId;
        }

        public long ProvaLegadoId { get; set; }
    }
}
