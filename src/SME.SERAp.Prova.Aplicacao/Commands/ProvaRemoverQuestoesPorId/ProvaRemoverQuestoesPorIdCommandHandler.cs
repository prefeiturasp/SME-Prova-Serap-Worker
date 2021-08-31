using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ProvaRemoverQuestoesPorIdCommandHandler : IRequestHandler<ProvaRemoverQuestoesPorIdCommand, bool>
    {
        private readonly IRepositorioQuestao repositorioQuestao;

        public ProvaRemoverQuestoesPorIdCommandHandler(IRepositorioQuestao repositorioQuestao)
        {
            this.repositorioQuestao = repositorioQuestao ?? throw new ArgumentNullException(nameof(repositorioQuestao));
        }
        public async Task<bool> Handle(ProvaRemoverQuestoesPorIdCommand request, CancellationToken cancellationToken)
        {
            return await repositorioQuestao.RemoverPorProvaId(request.Id);
        }
    }
}
