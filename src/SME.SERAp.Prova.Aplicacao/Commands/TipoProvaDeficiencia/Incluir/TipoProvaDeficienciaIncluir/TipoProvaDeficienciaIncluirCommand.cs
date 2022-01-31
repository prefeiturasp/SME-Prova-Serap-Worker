using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TipoProvaDeficienciaIncluirCommand : IRequest<long>
    {
        public TipoProvaDeficienciaIncluirCommand(TipoProvaDeficiencia tipoProvaDeficiencia)
        {
            TipoProvaDeficiencia = tipoProvaDeficiencia;
        }

        public TipoProvaDeficiencia TipoProvaDeficiencia { get; set; }
    }
}
