using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AlunoDeficienciaIncluirCommandHandler : IRequestHandler<AlunoDeficienciaIncluirCommand, long>
    {
        private readonly IRepositorioAlunoDeficiencia repositorioAlunoDeficiencia;

        public AlunoDeficienciaIncluirCommandHandler(IRepositorioAlunoDeficiencia repositorioAlunoDeficiencia)
        {
            this.repositorioAlunoDeficiencia = repositorioAlunoDeficiencia ?? throw new System.ArgumentNullException(nameof(repositorioAlunoDeficiencia));
        }

        public async Task<long> Handle(AlunoDeficienciaIncluirCommand request, CancellationToken cancellationToken)
        {
            return await repositorioAlunoDeficiencia.IncluirAsync(request.AlunoDeficiencia);
        }
    }
}
