using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ExcluirAdesaoPorProvaIdCommandHandler : IRequestHandler<ExcluirAdesaoPorProvaIdCommand, bool>
    {
        private readonly IRepositorioProvaAdesao repositorioProvaAdesao;

        public ExcluirAdesaoPorProvaIdCommandHandler(IRepositorioProvaAdesao repositorioProvaAdesao)
        {
            this.repositorioProvaAdesao = repositorioProvaAdesao ?? throw new ArgumentNullException(nameof(repositorioProvaAdesao));
        }

        public async Task<bool> Handle(ExcluirAdesaoPorProvaIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await repositorioProvaAdesao.ExcluirAdesaoPorProvaId(request.ProvaId);
                return true;
            }
            catch (Exception e)
            {
                throw new ArgumentException($"Excluir adesão prova:{request.ProvaId} -- Erro: {e.Message}.");
            }
        }
    }
}
