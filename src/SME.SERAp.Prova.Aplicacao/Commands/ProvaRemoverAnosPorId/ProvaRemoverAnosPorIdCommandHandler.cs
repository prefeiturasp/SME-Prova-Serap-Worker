using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ProvaRemoverAnosPorIdCommandHandler : IRequestHandler<ProvaRemoverAnosPorIdCommand, bool>
    {
        private readonly IRepositorioProvaAno repositorioProvaAno;

        public ProvaRemoverAnosPorIdCommandHandler(IRepositorioProvaAno repositorioProvaAno)
        {
            this.repositorioProvaAno = repositorioProvaAno ?? throw new ArgumentNullException(nameof(repositorioProvaAno));
        }
        public async Task<bool> Handle(ProvaRemoverAnosPorIdCommand request, CancellationToken cancellationToken)
        {
            return await repositorioProvaAno.RemoverAnosPorProvaIdAsync(request.Id);
        }
    }
}
