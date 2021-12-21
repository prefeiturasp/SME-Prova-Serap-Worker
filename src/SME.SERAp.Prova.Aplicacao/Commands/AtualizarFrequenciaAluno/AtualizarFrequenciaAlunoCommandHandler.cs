using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AtualizarFrequenciaAlunoCommandHandler : IRequestHandler<AtualizarFrequenciaAlunoCommand, bool>
    {
        private readonly IRepositorioProvaAluno repositorioProvaAluno;

        public AtualizarFrequenciaAlunoCommandHandler(IRepositorioProvaAluno repositorioProvaAluno)
        {
            this.repositorioProvaAluno = repositorioProvaAluno ?? throw new System.ArgumentNullException(nameof(repositorioProvaAluno));
        }
        public async Task<bool> Handle(AtualizarFrequenciaAlunoCommand request, CancellationToken cancellationToken)
        {
            await repositorioProvaAluno.AtualizarFrequenciaAlunoAsync(request.Id, request.Frequencia);
            return true;
        }
    }
}
