using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;

namespace SME.SERAp.Prova.Aplicacao
{
    public class QuestaoParaIncluirCommandHandler : IRequestHandler<QuestaoParaIncluirCommand, long>
    {
        private readonly IRepositorioQuestao repositorioQuestao;

        public QuestaoParaIncluirCommandHandler(IRepositorioQuestao repositorioQuestao)
        {
            this.repositorioQuestao = repositorioQuestao ?? throw new ArgumentNullException(nameof(repositorioQuestao));
        }
        
        public async Task<long> Handle(QuestaoParaIncluirCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await repositorioQuestao.IncluirAsync(request.Questao);
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
    }
}