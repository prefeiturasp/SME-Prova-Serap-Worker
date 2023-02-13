using MediatR;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterObjResultadoPspQuery : IRequest<ObjResultadoPspDto>
    {
        public ObterObjResultadoPspQuery(ObjResultadoPspDto resultado)
        {
            Resultado = resultado;
        }

        public ObjResultadoPspDto Resultado { get; set; }
    }
}
