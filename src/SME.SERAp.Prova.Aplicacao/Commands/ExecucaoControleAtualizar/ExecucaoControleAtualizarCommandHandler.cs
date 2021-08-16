using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExecucaoControleAtualizarCommandHandler : IRequestHandler<ExecucaoControleAtualizarCommand, long>
    {
        private readonly IRepositorioExecucaoControle repositorioExecucaoControle;

        public ExecucaoControleAtualizarCommandHandler(IRepositorioExecucaoControle repositorioExecucaoControle)
        {
            this.repositorioExecucaoControle = repositorioExecucaoControle ?? throw new ArgumentNullException(nameof(repositorioExecucaoControle));
        }
        public async Task<long> Handle(ExecucaoControleAtualizarCommand request, CancellationToken cancellationToken)
        {
            return await repositorioExecucaoControle.UpdateAsync(request.ExecucaoControle);
        }
    }
}
