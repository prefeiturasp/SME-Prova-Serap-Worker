using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class IncluirResultadoPspCommand : IRequest<bool>
    {
        public IncluirResultadoPspCommand(ObjResultadoPspDto resultado)
        {
            Resultado = resultado;
        }

        public ObjResultadoPspDto Resultado { get; set; }

    }
}
