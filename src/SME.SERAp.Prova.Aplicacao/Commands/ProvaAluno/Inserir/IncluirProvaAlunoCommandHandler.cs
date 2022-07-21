using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class IncluirProvaAlunoCommandHandler : IRequestHandler<IncluirProvaAlunoCommand, bool>
    {
        private readonly IRepositorioProvaAluno repositorioProvaAluno;

        public IncluirProvaAlunoCommandHandler(IRepositorioProvaAluno repositorioProvaAluno)
        {
            this.repositorioProvaAluno = repositorioProvaAluno ?? throw new System.ArgumentNullException(nameof(repositorioProvaAluno));
        }
        public async Task<bool> Handle(IncluirProvaAlunoCommand request, CancellationToken cancellationToken)
        {
            return await repositorioProvaAluno.SalvarAsync(request.ProvaAluno) > 0;
        }
    }
}
