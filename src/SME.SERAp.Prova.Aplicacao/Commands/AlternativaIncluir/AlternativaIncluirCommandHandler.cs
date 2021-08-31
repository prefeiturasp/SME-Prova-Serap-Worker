using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AlternativaIncluirCommandHandler : IRequestHandler<AlternativaIncluirCommand, long>
    {
        private readonly IRepositorioAlternativa repositorioAlternativa;

        public AlternativaIncluirCommandHandler(IRepositorioAlternativa repositorioAlternativa)
        {
            this.repositorioAlternativa = repositorioAlternativa ?? throw new ArgumentNullException(nameof(repositorioAlternativa));
        }
        
        public async Task<long> Handle(AlternativaIncluirCommand request, CancellationToken cancellationToken)
        {
            return await repositorioAlternativa.IncluirAsync(request.Alternativa);
        }
    }
}
