using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;

namespace SME.SERAp.Prova.Aplicacao.Queries
{
    public class ObterQuestaoArquivoIdPorQuestaoIdArquivoIdQueryHandler : IRequestHandler<ObterQuestaoArquivoIdPorQuestaoIdArquivoIdQuery, long>
    {
        private readonly IRepositorioQuestaoArquivo repositorioQuestaoArquivo;

        public ObterQuestaoArquivoIdPorQuestaoIdArquivoIdQueryHandler(IRepositorioQuestaoArquivo repositorioQuestaoArquivo)
        {
            this.repositorioQuestaoArquivo = repositorioQuestaoArquivo ?? throw new ArgumentNullException(nameof(repositorioQuestaoArquivo));
        }

        public async Task<long> Handle(ObterQuestaoArquivoIdPorQuestaoIdArquivoIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioQuestaoArquivo.ObterQuestaoArquivoIdPorQuestaoIdArquivoId(request.QuestaoId, request.ArquivoId);
        }
    }
}