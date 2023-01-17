using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TurmaAlunoHistoricoAlterarCommandHandler : IRequestHandler<TurmaAlunoHistoricoAlterarCommand, long>
    {
        private readonly IRepositorioTurmaAlunoHistorico repositorioTurmaAlunoHistorico;

        public TurmaAlunoHistoricoAlterarCommandHandler(IRepositorioTurmaAlunoHistorico repositorioTurmaAlunoHistorico)
        {
            this.repositorioTurmaAlunoHistorico = repositorioTurmaAlunoHistorico ?? throw new ArgumentNullException(nameof(repositorioTurmaAlunoHistorico));
        }

        public async Task<long> Handle(TurmaAlunoHistoricoAlterarCommand request, CancellationToken cancellationToken)
        {
            return await repositorioTurmaAlunoHistorico.UpdateAsync(request.TurmaAlunoHistorico);
        }
    }
}
