using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AlternativasParaIncluirCommandHandler : IRequestHandler<AlternativasParaIncluirCommand, long>
    {
        private readonly IRepositorioAlternativa repositorioAlternativa;

        public AlternativasParaIncluirCommandHandler(IRepositorioAlternativa repositorioAlternativa)
        {
            this.repositorioAlternativa = repositorioAlternativa ?? throw new ArgumentNullException(nameof(repositorioAlternativa));
        }
        
        public async Task<long> Handle(AlternativasParaIncluirCommand request, CancellationToken cancellationToken)
        {
            return await repositorioAlternativa.IncluirAsync(request.Alternativa);
        }
    }
}
