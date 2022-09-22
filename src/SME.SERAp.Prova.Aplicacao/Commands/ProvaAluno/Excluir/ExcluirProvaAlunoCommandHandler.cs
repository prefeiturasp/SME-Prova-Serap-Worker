using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
  public  class ExcluirProvaAlunoCommandHandler : IRequestHandler<ExcluirProvaAlunoCommand, int>
    {

        private readonly IRepositorioProvaAluno repositorioProvaAluno;

        public ExcluirProvaAlunoCommandHandler(IRepositorioProvaAluno repositorioProvaAluno)
        {
            this.repositorioProvaAluno = repositorioProvaAluno ?? throw new System.ArgumentNullException(nameof(repositorioProvaAluno));
        }

        public async Task<int> Handle(ExcluirProvaAlunoCommand request, CancellationToken cancellationToken)
        {
            return await repositorioProvaAluno.ExcluirProvaAlunoAsync(request.ProvaId, request.AlunoRa);
        }
    }
}