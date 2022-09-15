using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
  public class IncluirProvaAlunoReaberturaCommandHandler : IRequestHandler<IncluirProvaAlunoReaberturaCommand, long>
    {
        private readonly IRepositorioProvaAlunoReabertura repositorioProvaAlunoReabertura;

        public IncluirProvaAlunoReaberturaCommandHandler(IRepositorioProvaAlunoReabertura repositorioProvaAlunoReabertura)
        {
            this.repositorioProvaAlunoReabertura = repositorioProvaAlunoReabertura ?? throw new System.ArgumentNullException(nameof(repositorioProvaAlunoReabertura));
        }
        public async Task<long> Handle(IncluirProvaAlunoReaberturaCommand request, CancellationToken cancellationToken)
        {
            return await repositorioProvaAlunoReabertura.SalvarAsync(request.ProvaAlunoReabertura);
        }
    }
}