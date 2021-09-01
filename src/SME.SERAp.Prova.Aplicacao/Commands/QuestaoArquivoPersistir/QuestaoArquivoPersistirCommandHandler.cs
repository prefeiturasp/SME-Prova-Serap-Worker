using MediatR;
using SME.SERAp.Prova.Dados;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class QuestaoArquivoPersistirCommandHandler : IRequestHandler<QuestaoArquivoPersistirCommand, long>
    {
        private readonly IRepositorioQuestaoArquivo repositorioQuestaoArquivo;

        public QuestaoArquivoPersistirCommandHandler(IRepositorioQuestaoArquivo repositorioQuestaoArquivo)
        {
            this.repositorioQuestaoArquivo = repositorioQuestaoArquivo ?? throw new System.ArgumentNullException(nameof(repositorioQuestaoArquivo));
        }
        public async Task<long> Handle(QuestaoArquivoPersistirCommand request, CancellationToken cancellationToken)
        {
            return await repositorioQuestaoArquivo.IncluirAsync(new Dominio.QuestaoArquivo(request.ArquivoId, request.QuestaoId));
        }
    }
}
