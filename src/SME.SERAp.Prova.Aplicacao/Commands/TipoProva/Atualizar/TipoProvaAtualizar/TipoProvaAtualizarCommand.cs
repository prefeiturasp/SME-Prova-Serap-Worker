using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TipoProvaAtualizarCommand : IRequest<bool>
    {

        public TipoProvaAtualizarCommand(TipoProva tipoProva)
        {
            TipoProva = tipoProva;
        }

        public TipoProva TipoProva { get; set; }
    }
}
