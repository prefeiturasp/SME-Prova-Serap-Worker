using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class RemoverTipoProvaDeficienciaPorTipoProvaIdCommand : IRequest<bool>
    {
        public RemoverTipoProvaDeficienciaPorTipoProvaIdCommand(long tipoProvaId)
        {
            TipoProvaId = tipoProvaId;
        }
        public long TipoProvaId { get; set; }
    }
}
