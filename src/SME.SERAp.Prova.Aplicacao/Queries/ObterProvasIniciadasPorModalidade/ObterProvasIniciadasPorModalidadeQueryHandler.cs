using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterProvasIniciadasPorModalidadeQueryHandler : IRequestHandler<ObterProvasIniciadasPorModalidadeQuery, IEnumerable<ProvaAlunoDto>>
    {
        private readonly IRepositorioProva repositorioProva;
        public ObterProvasIniciadasPorModalidadeQueryHandler(IRepositorioProva repositorioProva)
        {
            this.repositorioProva = repositorioProva ?? throw new ArgumentNullException(nameof(repositorioProva));
        }
        public async Task<IEnumerable<ProvaAlunoDto>> Handle(ObterProvasIniciadasPorModalidadeQuery request, CancellationToken cancellationToken)
        {
            return await repositorioProva.ObterProvasIniciadasPorModalidadeAsync(request.Modalidade);
        }
    }
}
