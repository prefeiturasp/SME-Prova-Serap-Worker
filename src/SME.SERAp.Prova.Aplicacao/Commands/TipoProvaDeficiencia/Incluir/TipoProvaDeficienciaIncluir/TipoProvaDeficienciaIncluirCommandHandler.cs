using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TipoProvaDeficienciaIncluirCommandHandler : IRequestHandler<TipoProvaDeficienciaIncluirCommand, long>
    {
        private readonly IRepositorioTipoProvaDeficiencia repositorioTipoProvaDeficiencia;

        public TipoProvaDeficienciaIncluirCommandHandler(IRepositorioTipoProvaDeficiencia repositorioTipoProvaDeficiencia)
        {
            this.repositorioTipoProvaDeficiencia = repositorioTipoProvaDeficiencia ?? throw new System.ArgumentNullException(nameof(repositorioTipoProvaDeficiencia));
        }

        public async Task<long> Handle(TipoProvaDeficienciaIncluirCommand request, CancellationToken cancellationToken)
        {
            return await repositorioTipoProvaDeficiencia.IncluirAsync(request.TipoProvaDeficiencia);
        }
    }
}
