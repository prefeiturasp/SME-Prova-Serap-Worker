using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class IncluirDownloadProvaCommandHandler : IRequestHandler<IncluirDownloadProvaCommand, bool>
    {
        private readonly IRepositorioDownloadProvaAluno repositorioDownloadProvaAluno;

        public IncluirDownloadProvaCommandHandler(IRepositorioDownloadProvaAluno repositorioDownloadProvaAluno)
        {
            this.repositorioDownloadProvaAluno = repositorioDownloadProvaAluno ?? throw new System.ArgumentNullException(nameof(repositorioDownloadProvaAluno));
        }

        public async Task<bool> Handle(IncluirDownloadProvaCommand request, CancellationToken cancellationToken)
        { 
            var id = await repositorioDownloadProvaAluno.IncluirAsync(new Dominio.DownloadProvaAluno(request.DownloadProvaAlunoDto.ProvaId,
                request.DownloadProvaAlunoDto.AlunoRa,
                request.DownloadProvaAlunoDto.DispositivoId,
                request.DownloadProvaAlunoDto.TipoDispositivo,
                request.DownloadProvaAlunoDto.ModeloDispositivo,
                request.DownloadProvaAlunoDto.Versao,
                request.Situacao,
                request.DownloadProvaAlunoDto.DataHora.AddHours(3),
                request.DownloadProvaAlunoDto.Codigo)
                );

            return id > 0;
        }
    }
}

