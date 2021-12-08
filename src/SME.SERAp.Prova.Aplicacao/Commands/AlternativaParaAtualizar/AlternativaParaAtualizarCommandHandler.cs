using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AlternativaParaAtualizarCommandHandler : IRequestHandler<AlternativaParaAtualizarCommand, long>
    {
        private readonly IRepositorioAlternativa repositorioAlternativa;

        public AlternativaParaAtualizarCommandHandler(IRepositorioAlternativa repositorioAlternativa)
        {
            this.repositorioAlternativa = repositorioAlternativa ?? throw new ArgumentNullException(nameof(repositorioAlternativa));
        }

        public async Task<long> Handle(AlternativaParaAtualizarCommand request, CancellationToken cancellationToken)
        {
            return await repositorioAlternativa.UpdateAsync(request.Alternativa);
        }
    }
}