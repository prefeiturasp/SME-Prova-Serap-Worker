using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TipoProvaIncluirCommandHandler : IRequestHandler<TipoProvaIncluirCommand, long>
    {
        
        private readonly IRepositorioTipoProva repositorioTipoProva;

        public TipoProvaIncluirCommandHandler(IRepositorioTipoProva repositorioTipoProva)
        {
            this.repositorioTipoProva = repositorioTipoProva ?? throw new System.ArgumentNullException(nameof(repositorioTipoProva));
        }

        public async Task<long> Handle(TipoProvaIncluirCommand request, CancellationToken cancellationToken)
        {
            var tipoProvaIncluir = new TipoProva(request.TipoProva.LegadoId, request.TipoProva.Descricao, request.TipoProva.ParaEstudanteComDeficiencia);
            return await repositorioTipoProva.IncluirAsync(tipoProvaIncluir);
        }
    }
}
