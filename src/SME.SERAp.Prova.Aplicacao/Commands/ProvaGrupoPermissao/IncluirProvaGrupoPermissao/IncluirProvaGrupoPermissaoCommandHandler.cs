using MediatR;
using SME.SERAp.Prova.Dados.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.Commands
{ 
   public class IncluirProvaGrupoPermissaoCommandHandler : IRequestHandler<IncluirProvaGrupoPermissaoCommand, bool>
{
    private readonly IRepositorioProvaGrupoPermissaoEntity repositorioProvaGrupoPermissao;

    public IncluirProvaGrupoPermissaoCommandHandler(IRepositorioProvaGrupoPermissaoEntity repositorioProvaGrupoPermissao)
    {
        this.repositorioProvaGrupoPermissao = repositorioProvaGrupoPermissao ?? throw new System.ArgumentNullException(nameof(repositorioProvaGrupoPermissao));
    }

    public async Task<bool> Handle(IncluirProvaGrupoPermissaoCommand request, CancellationToken cancellationToken)
    {
        await repositorioProvaGrupoPermissao.InserirVariosAsync(request.ListaProvaGrupoPermissao);
        return true;
    }

}
}
