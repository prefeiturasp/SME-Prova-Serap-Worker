using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterDadosAmostraProvaTaiQueryHandler : IRequestHandler<ObterDadosAmostraProvaTaiQuery, IEnumerable<AmostraProvaTaiDto>>
    {   
        private readonly IRepositorioProvaLegado repositorioProvaLegado;

        public ObterDadosAmostraProvaTaiQueryHandler(IRepositorioProvaLegado repositorioProvaLegado)
        {
            this.repositorioProvaLegado = repositorioProvaLegado ?? throw new ArgumentNullException(nameof(repositorioProvaLegado));
        }

        public async Task<IEnumerable<AmostraProvaTaiDto>> Handle(ObterDadosAmostraProvaTaiQuery request, CancellationToken cancellationToken)
        {
            return await repositorioProvaLegado.ObterDadosAmostraProvaTai(request.ProvaLegadoId);
        }
    }
}
