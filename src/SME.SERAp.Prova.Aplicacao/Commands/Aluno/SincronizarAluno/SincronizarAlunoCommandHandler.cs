using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class SincronizarAlunoCommandHandler : IRequestHandler<SincronizarAlunoCommand, long>
    {
        private readonly IRepositorioAluno repositorioAluno;

        public SincronizarAlunoCommandHandler(IRepositorioAluno repositorioAluno)
        {
            this.repositorioAluno = repositorioAluno ?? throw new ArgumentNullException(nameof(repositorioAluno));
        }

        public async Task<long> Handle(SincronizarAlunoCommand request, CancellationToken cancellationToken)
             => await repositorioAluno.InserirOuAtualizarAlunoAsync(request.AlunoEol);
    }
}
