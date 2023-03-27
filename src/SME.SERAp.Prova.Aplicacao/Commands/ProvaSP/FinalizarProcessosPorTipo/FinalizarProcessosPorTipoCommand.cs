using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class FinalizarProcessosPorTipoCommand : IRequest<bool>
    {
        public FinalizarProcessosPorTipoCommand(TipoResultadoPsp tipoResultadoProcesso)
        {
            TipoResultadoProcesso = tipoResultadoProcesso;
        }

        public TipoResultadoPsp TipoResultadoProcesso { get; set; }
    }
}
