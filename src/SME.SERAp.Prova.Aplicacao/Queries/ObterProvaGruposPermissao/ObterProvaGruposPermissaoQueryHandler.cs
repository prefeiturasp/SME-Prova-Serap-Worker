using MediatR;
using SME.SERAp.Prova.Dados.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.Queries
{
    public class ObterProvaGruposPermissaoQueryHandler : IRequestHandler<ObterProvaGruposPermissaoQuery, IEnumerable<Dominio.ProvaGrupoPermissao>>
    {
        private readonly IRepositorioProvaGrupoPermissao repositorioProvaGrupoPermissao;
        public ObterProvaGruposPermissaoQueryHandler(IRepositorioProvaGrupoPermissao repositorioProvaGrupoPermissao)
        {
            this.repositorioProvaGrupoPermissao = repositorioProvaGrupoPermissao ?? throw new ArgumentNullException(nameof(repositorioProvaGrupoPermissao));
        }
        public async Task<IEnumerable<Dominio.ProvaGrupoPermissao>> Handle(ObterProvaGruposPermissaoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioProvaGrupoPermissao.ObterPorProvaIdAsync(request.ProvaId);
        }
    }
}

