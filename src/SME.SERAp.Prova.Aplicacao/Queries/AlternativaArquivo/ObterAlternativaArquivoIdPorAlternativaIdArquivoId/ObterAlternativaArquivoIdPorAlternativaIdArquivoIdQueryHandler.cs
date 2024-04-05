using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;

namespace SME.SERAp.Prova.Aplicacao.Queries
{
    public class ObterAlternativaArquivoIdPorAlternativaIdArquivoIdQueryHandler : IRequestHandler<ObterAlternativaArquivoIdPorAlternativaIdArquivoIdQuery, long>
    {
        private readonly IRepositorioAlternativaArquivo repositorioAlternativaArquivo;

        public ObterAlternativaArquivoIdPorAlternativaIdArquivoIdQueryHandler(IRepositorioAlternativaArquivo repositorioAlternativaArquivo)
        {
            this.repositorioAlternativaArquivo = repositorioAlternativaArquivo ?? throw new ArgumentNullException(nameof(repositorioAlternativaArquivo));
        }

        public async Task<long> Handle(ObterAlternativaArquivoIdPorAlternativaIdArquivoIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAlternativaArquivo.ObterAlternativaArquivoIdPorAlternativaIdArquivoId(request.AlternativaId, request.ArquivoId);
        }
    }
}