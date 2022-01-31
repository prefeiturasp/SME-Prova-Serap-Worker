using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class RemoverTipoProvaDeficienciaPorTipoProvaIdCommandHandler : IRequestHandler<RemoverTipoProvaDeficienciaPorTipoProvaIdCommand, bool>
    {

        private readonly IRepositorioTipoProvaDeficiencia repositorioTipoProvaDeficiencia;

        public RemoverTipoProvaDeficienciaPorTipoProvaIdCommandHandler(IRepositorioTipoProvaDeficiencia repositorioTipoProvaDeficiencia)
        {
            this.repositorioTipoProvaDeficiencia = repositorioTipoProvaDeficiencia ?? throw new System.ArgumentNullException(nameof(repositorioTipoProvaDeficiencia));
        }

        public async Task<bool> Handle(RemoverTipoProvaDeficienciaPorTipoProvaIdCommand request, CancellationToken cancellationToken)
        {
            return await repositorioTipoProvaDeficiencia.RemoverPorTipoProvaId(request.TipoProvaId);
        }
    }
}
