﻿using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class IncluirProvaAlunoCommandHandler : IRequestHandler<IncluirProvaAlunoCommand, long>
    {
        private readonly IRepositorioProvaAluno repositorioProvaAluno;

        public IncluirProvaAlunoCommandHandler(IRepositorioProvaAluno repositorioProvaAluno)
        {
            this.repositorioProvaAluno = repositorioProvaAluno ?? throw new System.ArgumentNullException(nameof(repositorioProvaAluno));
        }
        public async Task<long> Handle(IncluirProvaAlunoCommand request, CancellationToken cancellationToken)
        {
            return await repositorioProvaAluno.SalvarAsync(request.ProvaAluno);
        }
    }
}
