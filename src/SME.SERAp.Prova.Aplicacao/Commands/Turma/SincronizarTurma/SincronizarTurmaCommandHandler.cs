using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.Dtos;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class SincronizarTurmaCommandHandler : IRequestHandler<SincronizarTurmaCommand, long>
    {
        private readonly IRepositorioTurma repositorioTurma;

        public SincronizarTurmaCommandHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<long> Handle(SincronizarTurmaCommand request, CancellationToken cancellationToken)
            => await repositorioTurma.InserirOuAtualizarTurmaAsync(request.TurmaSgp);
    }
}
