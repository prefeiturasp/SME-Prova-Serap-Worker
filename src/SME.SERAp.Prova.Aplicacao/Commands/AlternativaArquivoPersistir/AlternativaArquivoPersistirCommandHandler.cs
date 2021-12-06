using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AlternativaArquivoPersistirCommandHandler : IRequestHandler<AlternativaArquivoPersistirCommand, long>
    {
        private readonly IRepositorioAlternativaArquivo repositorioAlternativaArquivo;

        public AlternativaArquivoPersistirCommandHandler(IRepositorioAlternativaArquivo repositorioAlternativaArquivo)
        {
            this.repositorioAlternativaArquivo = repositorioAlternativaArquivo ?? throw new System.ArgumentNullException(nameof(repositorioAlternativaArquivo));
        }
        public async Task<long> Handle(AlternativaArquivoPersistirCommand request, CancellationToken cancellationToken)
        {
            return await repositorioAlternativaArquivo.IncluirAsync(new Dominio.AlternativaArquivo(request.ArquivoId, request.AlternativaId));
        }
    }
}
