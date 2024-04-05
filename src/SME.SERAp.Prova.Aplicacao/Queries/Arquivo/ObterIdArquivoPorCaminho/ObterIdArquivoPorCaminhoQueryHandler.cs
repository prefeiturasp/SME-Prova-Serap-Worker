using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;

namespace SME.SERAp.Prova.Aplicacao.Queries
{
    public class ObterIdArquivoPorCaminhoQueryHandler : IRequestHandler<ObterIdArquivoPorCaminhoQuery, long>
    {
        private readonly IRepositorioArquivo repositorioArquivo;

        public ObterIdArquivoPorCaminhoQueryHandler(IRepositorioArquivo repositorioArquivo)
        {
            this.repositorioArquivo = repositorioArquivo ?? throw new ArgumentNullException(nameof(repositorioArquivo));
        }

        public async Task<long> Handle(ObterIdArquivoPorCaminhoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioArquivo.ObterIdArquivoPorCaminho(request.Caminho);
        }
    }
}