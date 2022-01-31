using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class InserirListaProvaAdesaoCommandHandler : IRequestHandler<InserirListaProvaAdesaoCommand, bool>
    {
        private readonly IRepositorioProvaAdesaoEntity repositorioProvaAdesaoEntity;

        public InserirListaProvaAdesaoCommandHandler(IRepositorioProvaAdesaoEntity repositorioProvaAdesaoEntity)
        {
            this.repositorioProvaAdesaoEntity = repositorioProvaAdesaoEntity ?? throw new ArgumentNullException(nameof(repositorioProvaAdesaoEntity));
        }

        public async Task<bool> Handle(InserirListaProvaAdesaoCommand request, CancellationToken cancellationToken)
        {
            if (request.ListaProvaAdesao is null || !request.ListaProvaAdesao.Any())
                return default;

            string provaId = request.ListaProvaAdesao.Select(a => a.ProvaId).FirstOrDefault().ToString();

            try
            {
                await repositorioProvaAdesaoEntity.InserirVariosAsync(request.ListaProvaAdesao);
                return true;
            }
            catch(Exception e)
            {
                throw new ArgumentException($"Inserir adesão prova:{provaId} -- Erro: {e.Message}.");
            }
        }
    }
}
