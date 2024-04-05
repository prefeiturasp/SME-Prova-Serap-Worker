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
            await RemoverAlternativasArquivos(request.Id);
            await repositorioAlternativa.RemoverPorProvaId(request.Id);
            return true;
        }
        
        private async Task RemoverAlternativasArquivos(long provaId)
        {
            var alternativasArquivos = await repositorioAlternativaArquivo.ObterArquivosPorProvaIdAsync(provaId);
            if (alternativasArquivos != null && alternativasArquivos.Any())
            {
                var idsAlternativasArquivos = alternativasArquivos.Select(a => a.Id);
                await repositorioAlternativaArquivo.RemoverPorIdsAsync(idsAlternativasArquivos.ToArray());

                var idsArquivos = alternativasArquivos.Select(a => a.ArquivoId).ToArray();
                await repositorioArquivo.RemoverPorIdsAsync(idsArquivos);
            }            
        }        
    }
}
