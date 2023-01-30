using MediatR;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    internal class ObterTipoResultadoPspQueryHandler : IRequestHandler<ObterTipoResultadoPspQuery, ArquivoResultadoPspDto>
    {

        private readonly IRepositorioArquivoResultadoPsp repositorioArquivoResultadoPsp;

        public ObterTipoResultadoPspQueryHandler(IRepositorioArquivoResultadoPsp repositorioArquivoResultadoPsp)
        {
            this.repositorioArquivoResultadoPsp = repositorioArquivoResultadoPsp ?? throw new System.ArgumentNullException(nameof(repositorioArquivoResultadoPsp));
        }

        public async Task<ArquivoResultadoPspDto> Handle(ObterTipoResultadoPspQuery request, CancellationToken cancellationToken)
        {
            return await repositorioArquivoResultadoPsp.ObterArquivoResultadoPspPorId(request.Id);
        }
    }
}