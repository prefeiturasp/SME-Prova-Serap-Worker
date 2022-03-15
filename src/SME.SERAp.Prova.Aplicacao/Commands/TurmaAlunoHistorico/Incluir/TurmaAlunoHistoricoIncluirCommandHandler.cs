using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TurmaAlunoHistoricoIncluirCommandHandler : IRequestHandler<TurmaAlunoHistoricoIncluirCommand, long>
    {
        private readonly IRepositorioTurmaAlunoHistorico repositorioTurmaAlunoHistorico;

        public TurmaAlunoHistoricoIncluirCommandHandler(IRepositorioTurmaAlunoHistorico repositorioTurmaAlunoHistorico)
        {
            this.repositorioTurmaAlunoHistorico = repositorioTurmaAlunoHistorico ?? throw new ArgumentNullException(nameof(repositorioTurmaAlunoHistorico));
        }

        public async Task<long> Handle(TurmaAlunoHistoricoIncluirCommand request, CancellationToken cancellationToken)
        {
            return await repositorioTurmaAlunoHistorico.IncluirAsync(request.TurmaAlunoHistorico);
        }
    }
}
