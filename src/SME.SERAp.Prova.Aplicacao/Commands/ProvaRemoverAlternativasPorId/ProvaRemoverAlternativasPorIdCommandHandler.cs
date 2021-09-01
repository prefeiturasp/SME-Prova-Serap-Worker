using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ProvaRemoverAlternativasPorIdCommandHandler : IRequestHandler<ProvaRemoverAlternativasPorIdCommand, bool>
    {
        private readonly IRepositorioAlternativa repositorioAlternativa;

        public ProvaRemoverAlternativasPorIdCommandHandler(IRepositorioAlternativa repositorioAlternativa)
        {
            this.repositorioAlternativa = repositorioAlternativa ?? throw new ArgumentNullException(nameof(repositorioAlternativa));
        }
        public async Task<bool> Handle(ProvaRemoverAlternativasPorIdCommand request, CancellationToken cancellationToken)
        {
            return await repositorioAlternativa.RemoverPorProvaId(request.Id);
        }
    }
}
