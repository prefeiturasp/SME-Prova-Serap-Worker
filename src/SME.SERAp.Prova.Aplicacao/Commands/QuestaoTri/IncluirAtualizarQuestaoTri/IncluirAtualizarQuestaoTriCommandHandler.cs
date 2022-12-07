using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class IncluirAtualizarQuestaoTriCommandHandler : IRequestHandler<IncluirAtualizarQuestaoTriCommand, bool>
    {

        private readonly IRepositorioQuestaoTri repositorioQuestaoTri;

        public IncluirAtualizarQuestaoTriCommandHandler(IRepositorioQuestaoTri repositorioQuestaoTri)
        {
            this.repositorioQuestaoTri = repositorioQuestaoTri ?? throw new ArgumentException(nameof(repositorioQuestaoTri));
        }

        public async Task<bool> Handle(IncluirAtualizarQuestaoTriCommand request, CancellationToken cancellationToken)
        {
            var questaoTri = request.QuestaoTri;
            long retorno = 0;

            if (questaoTri != null && questaoTri.Id == 0)
                retorno = await repositorioQuestaoTri.IncluirAsync(questaoTri);
            if (questaoTri != null && questaoTri.Id > 0)
                retorno = await repositorioQuestaoTri.UpdateAsync(questaoTri);
            
            return retorno > 0;
        }
    }
}
