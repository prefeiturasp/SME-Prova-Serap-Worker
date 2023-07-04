using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AtualizarTurmaCommandHandler : IRequestHandler<AtualizarTurmaCommand, long>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public AtualizarTurmaCommandHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<long> Handle(AtualizarTurmaCommand request, CancellationToken cancellationToken)
        {
            return await repositorioTurma.UpdateAsync(request.Turma);
        }
    }
}