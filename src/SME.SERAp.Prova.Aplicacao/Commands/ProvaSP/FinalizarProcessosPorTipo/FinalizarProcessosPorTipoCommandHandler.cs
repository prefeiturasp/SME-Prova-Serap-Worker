using MediatR;
using SME.SERAp.Prova.Dados.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class FinalizarProcessosPorTipoCommandHandler : IRequestHandler<FinalizarProcessosPorTipoCommand, bool>
    {
        private readonly IRepositorioArquivoResultadoPsp repositorioArquivoResultadoPsp;

        public FinalizarProcessosPorTipoCommandHandler(IRepositorioArquivoResultadoPsp repositorioArquivoResultadoPsp)
        {
            this.repositorioArquivoResultadoPsp = repositorioArquivoResultadoPsp ?? throw new System.ArgumentNullException(nameof(repositorioArquivoResultadoPsp));
        }
        public async Task<bool> Handle(FinalizarProcessosPorTipoCommand request, CancellationToken cancellationToken)
        {
            await repositorioArquivoResultadoPsp.FinalizarProcessosPorTipo(request.TipoResultadoProcesso);
            return true;
        }
    }
}
