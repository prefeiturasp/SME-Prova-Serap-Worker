using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class RemoverAlunoDeficienciaPorAlunoRaCommandHandler : IRequestHandler<RemoverAlunoDeficienciaPorAlunoRaCommand, bool>
    {
        
        private readonly IRepositorioAlunoDeficiencia repositorioAlunoDeficiencia;

        public RemoverAlunoDeficienciaPorAlunoRaCommandHandler(IRepositorioAlunoDeficiencia repositorioAlunoDeficiencia)
        {
            this.repositorioAlunoDeficiencia = repositorioAlunoDeficiencia ?? throw new System.ArgumentNullException(nameof(repositorioAlunoDeficiencia));
        }

        public async Task<bool> Handle(RemoverAlunoDeficienciaPorAlunoRaCommand request, CancellationToken cancellationToken)
        {
            return await repositorioAlunoDeficiencia.RemoverPorAlunoRa(request.AlunoRa);
        }
    }
}
