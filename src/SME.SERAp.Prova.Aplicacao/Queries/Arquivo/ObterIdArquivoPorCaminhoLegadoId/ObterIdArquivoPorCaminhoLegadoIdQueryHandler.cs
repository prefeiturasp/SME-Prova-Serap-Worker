using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.Queries
{
    public class ObterIdArquivoPorCaminhoLegadoIdQueryHandler : IRequestHandler<ObterIdArquivoPorCaminhoLegadoIdQuery, long>
    {
        private readonly IRepositorioArquivo repositorioArquivo;

        public ObterIdArquivoPorCaminhoLegadoIdQueryHandler(IRepositorioArquivo repositorioArquivo)
        {
            this.repositorioArquivo = repositorioArquivo ?? throw new ArgumentNullException(nameof(repositorioArquivo));
        }

        public async Task<long> Handle(ObterIdArquivoPorCaminhoLegadoIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioArquivo.ObterIdArquivoPorCaminhoLegadoId(request.Caminho, request.LegadoId);
        }
    }
}
