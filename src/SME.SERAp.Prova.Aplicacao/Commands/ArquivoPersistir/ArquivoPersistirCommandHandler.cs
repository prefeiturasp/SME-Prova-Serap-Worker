using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ArquivoPersistirCommandHandler : IRequestHandler<ArquivoPersistirCommand, long>
    {
        private readonly IRepositorioArquivo repositorioArquivo;

        public ArquivoPersistirCommandHandler(IRepositorioArquivo repositorioArquivo)
        {
            this.repositorioArquivo = repositorioArquivo ?? throw new System.ArgumentNullException(nameof(repositorioArquivo));
        }
        public async Task<long> Handle(ArquivoPersistirCommand request, CancellationToken cancellationToken)
        {
            return await repositorioArquivo.IncluirAsync(request.Arquivo);
        }
    }
}
