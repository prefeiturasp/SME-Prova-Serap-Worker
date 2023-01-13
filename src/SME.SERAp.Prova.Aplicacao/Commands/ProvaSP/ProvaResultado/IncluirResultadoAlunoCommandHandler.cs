using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SME.SERAp.Prova.Dados.Interfaces;

namespace SME.SERAp.Prova.Aplicacao.Commands.ProvaSP.ProvaResultado
{
    public class IncluirResultadoAlunoCommandHandler : IRequestHandler<IncluirResultadoAlunoCommand, long>
    {

        private readonly IRepositorioResultadoAluno respositorioResultadoAluno;

        public IncluirResultadoAlunoCommandHandler(IRepositorioResultadoAluno respositorioResultadoAluno)
        {
            this.respositorioResultadoAluno = respositorioResultadoAluno ?? throw new System.ArgumentNullException(nameof(respositorioResultadoAluno));
        }

        public async Task<long> Handle(IncluirResultadoAlunoCommand request, CancellationToken cancellationToken)
        {
            return await respositorioResultadoAluno.IncluirAsync(request.ResultadoAluno);
        }
    }
}