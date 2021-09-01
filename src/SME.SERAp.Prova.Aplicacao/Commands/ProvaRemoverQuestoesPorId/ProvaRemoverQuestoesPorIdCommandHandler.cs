using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ProvaRemoverQuestoesPorIdCommandHandler : IRequestHandler<ProvaRemoverQuestoesPorIdCommand, bool>
    {
        private readonly IRepositorioQuestao repositorioQuestao;
        private readonly IRepositorioArquivo repositorioArquivo;
        private readonly IRepositorioQuestaoArquivo repositorioQuestaoArquivo;

        public ProvaRemoverQuestoesPorIdCommandHandler(IRepositorioQuestao repositorioQuestao, IRepositorioArquivo repositorioArquivo,
            IRepositorioQuestaoArquivo repositorioQuestaoArquivo)
        {
            this.repositorioQuestao = repositorioQuestao ?? throw new ArgumentNullException(nameof(repositorioQuestao));
            this.repositorioArquivo = repositorioArquivo ?? throw new ArgumentNullException(nameof(repositorioArquivo));
            this.repositorioQuestaoArquivo = repositorioQuestaoArquivo ?? throw new ArgumentNullException(nameof(repositorioQuestaoArquivo));
        }
        public async Task<bool> Handle(ProvaRemoverQuestoesPorIdCommand request, CancellationToken cancellationToken)
        {

            //TODO: IMPLEMENTAR TRANSAÇÃO
            var questoesArquivos = await repositorioQuestaoArquivo.ObterArquivosPorProvaIdAsync(request.Id);
            if (questoesArquivos.Any())
            {
                var idsQuestosArquivos = questoesArquivos.Select(a => a.Id).ToArray();
                await repositorioQuestaoArquivo.RemoverPorIdsAsync(idsQuestosArquivos);
                var idsArquivos = questoesArquivos.Select(a => a.ArquivoId).ToArray();
                await repositorioArquivo.RemoverPorIdsAsync(idsArquivos);
            }

            await repositorioQuestao.RemoverPorProvaIdAsync(request.Id);

            return true;
        }
    }
}
