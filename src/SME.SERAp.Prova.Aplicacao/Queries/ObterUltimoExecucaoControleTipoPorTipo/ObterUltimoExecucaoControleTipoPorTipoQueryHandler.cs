using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUltimoExecucaoControleTipoPorTipoQueryHandler : IRequestHandler<ObterUltimoExecucaoControleTipoPorTipoQuery, ExecucaoControle>
    {
        private readonly IRepositorioExecucaoControle repositorioExecucaoControle;

        public ObterUltimoExecucaoControleTipoPorTipoQueryHandler(IRepositorioExecucaoControle repositorioExecucaoControle)
        {
            this.repositorioExecucaoControle = repositorioExecucaoControle ?? throw new System.ArgumentNullException(nameof(repositorioExecucaoControle));
        }
        public async Task<ExecucaoControle> Handle(ObterUltimoExecucaoControleTipoPorTipoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioExecucaoControle.ObterUltimaExecucaoPorTipoAsync(request.Tipo);
        }
    }
}
