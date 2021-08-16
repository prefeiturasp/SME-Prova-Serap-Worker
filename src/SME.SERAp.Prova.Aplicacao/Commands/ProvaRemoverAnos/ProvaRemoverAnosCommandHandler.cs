using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ProvaRemoverAnosCommandHandler : IRequestHandler<ProvaRemoverAnosCommand, bool>
    {
        private readonly IRepositorioProvaAno repositorioProvaAno;

        public ProvaRemoverAnosCommandHandler(IRepositorioProvaAno repositorioProvaAno)
        {
            this.repositorioProvaAno = repositorioProvaAno ?? throw new ArgumentNullException(nameof(repositorioProvaAno));
        }
        public async Task<bool> Handle(ProvaRemoverAnosCommand request, CancellationToken cancellationToken)
        {
            return await repositorioProvaAno.RemoverAnosPorProvaIdAsync(request.Id);
        }
    }
}
