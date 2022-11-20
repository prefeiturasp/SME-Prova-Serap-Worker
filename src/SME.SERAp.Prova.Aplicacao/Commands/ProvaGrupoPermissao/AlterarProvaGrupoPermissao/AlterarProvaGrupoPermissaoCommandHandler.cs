using MediatR;
using SME.SERAp.Prova.Dados.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AlterarProvaGrupoPermissaoCommandHandler : IRequestHandler<AlterarProvaGrupoPermissaoCommand, bool>
    {
        private readonly IRepositorioProvaGrupoPermissaoEntity repositorioProvaGrupoPermissao;

        public AlterarProvaGrupoPermissaoCommandHandler(IRepositorioProvaGrupoPermissaoEntity repositorioProvaGrupoPermissao)
        {
            this.repositorioProvaGrupoPermissao = repositorioProvaGrupoPermissao ?? throw new System.ArgumentNullException(nameof(repositorioProvaGrupoPermissao));
        }

        public async Task<bool> Handle(AlterarProvaGrupoPermissaoCommand request, CancellationToken cancellationToken)
        {
            await repositorioProvaGrupoPermissao.AlterarVariosAsync(request.ListaProvaGrupoPermissao);
            return true;
        }
    }
}