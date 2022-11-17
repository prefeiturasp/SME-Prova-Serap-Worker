using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.Queries
{
 public   class ObterPermissaoProvaLegadoQueryHandler : IRequestHandler<ObterPermissoesProvaLegadoQuery, IEnumerable<ProvaGrupoPermissaoDto>>
    {
        private readonly IRepositorioProvaLegado repositorioProvaLegado;

        public ObterPermissaoProvaLegadoQueryHandler(IRepositorioProvaLegado repositorioProvaLegado)
        {
            this.repositorioProvaLegado = repositorioProvaLegado ?? throw new ArgumentNullException(nameof(repositorioProvaLegado));
        }
        public async Task<IEnumerable<ProvaGrupoPermissaoDto>> Handle(ObterPermissoesProvaLegadoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioProvaLegado.ObterDadosProvaGrupoPermissaoPorId(request.ProvaLegadoId);
        }
    }
}

