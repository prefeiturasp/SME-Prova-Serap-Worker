using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AlterarResultadoPspCommand : IRequest<bool>
    {
        public AlterarResultadoPspCommand(ObjResultadoPspDto resultado)
        {
            Resultado = resultado;
        }

        public ObjResultadoPspDto Resultado { get; set; }
    }
}
