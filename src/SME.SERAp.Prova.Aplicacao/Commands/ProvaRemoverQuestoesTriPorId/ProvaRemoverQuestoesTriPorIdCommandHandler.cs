using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ProvaRemoverQuestoesTriPorIdCommandHandler : IRequestHandler<ProvaRemoverQuestoesTriPorIdCommand, bool>
    {
        private readonly IRepositorioQuestaoTri repositorioQuestaoTri;

        public ProvaRemoverQuestoesTriPorIdCommandHandler(IRepositorioQuestaoTri repositorioQuestaoTri)
        {
            this.repositorioQuestaoTri = repositorioQuestaoTri ?? throw new ArgumentNullException(nameof(repositorioQuestaoTri));
        }

        public async Task<bool> Handle(ProvaRemoverQuestoesTriPorIdCommand request, CancellationToken cancellationToken)
        {
            return await repositorioQuestaoTri.RemoverPorProvaIdAsync(request.Id);
        }
    }
}
