using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ProvaRemoverAlternativasPorIdCommandHandler : IRequestHandler<ProvaRemoverAlternativasPorIdCommand, bool>
    {
        private readonly IRepositorioAlternativa repositorioAlternativa;
        private readonly IRepositorioAlternativaArquivo repositorioAlternativaArquivo;
        private readonly IRepositorioArquivo repositorioArquivo;

        public ProvaRemoverAlternativasPorIdCommandHandler(IRepositorioAlternativa repositorioAlternativa, 
            IRepositorioAlternativaArquivo repositorioAlternativaArquivo, IRepositorioArquivo repositorioArquivo)
        {
            this.repositorioAlternativa = repositorioAlternativa ?? throw new ArgumentNullException(nameof(repositorioAlternativa));
            this.repositorioAlternativaArquivo = repositorioAlternativaArquivo ?? throw new ArgumentNullException(nameof(repositorioAlternativaArquivo));
            this.repositorioArquivo = repositorioArquivo ?? throw new ArgumentNullException(nameof(repositorioArquivo));
        }
        public async Task<bool> Handle(ProvaRemoverAlternativasPorIdCommand request, CancellationToken cancellationToken)
        {
            var alternativaArquivos = await repositorioAlternativaArquivo.ObterArquivosPorProvaIdAsync(request.Id);
            if (alternativaArquivos.Any())
            {
                var idsAlternativasArquivos = alternativaArquivos.Select(a => a.Id).ToArray();
                await repositorioAlternativaArquivo.RemoverPorIdsAsync(idsAlternativasArquivos);
                var idsArquivos = alternativaArquivos.Select(a => a.ArquivoId).ToArray();
                await repositorioArquivo.RemoverPorIdsAsync(idsArquivos);
            }

            await repositorioAlternativa.RemoverPorProvaId(request.Id);

            return true;
        }
    }
}
