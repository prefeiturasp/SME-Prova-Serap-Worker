using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TipoProvaIncluirCommand : IRequest<long>
    {
        public TipoProvaIncluirCommand(TipoProva tipoProva)
        {
            TipoProva = tipoProva;
        }

        public TipoProva TipoProva { get; set; }
    }
}
