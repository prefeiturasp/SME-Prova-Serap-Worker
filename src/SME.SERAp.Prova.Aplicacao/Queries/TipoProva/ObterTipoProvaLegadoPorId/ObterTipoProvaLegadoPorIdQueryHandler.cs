using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTipoProvaLegadoPorIdQueryHandler : IRequestHandler<ObterTipoProvaLegadoPorIdQuery, TipoProva>
    {
        private readonly IRepositorioGeralSerapLegado repositorioGeralSerapLegado;
        public ObterTipoProvaLegadoPorIdQueryHandler(IRepositorioGeralSerapLegado repositorioGeralSerapLegado)
        {
            this.repositorioGeralSerapLegado = repositorioGeralSerapLegado ?? throw new ArgumentNullException(nameof(repositorioGeralSerapLegado));
        }

        public async Task<TipoProva> Handle(ObterTipoProvaLegadoPorIdQuery request,
            CancellationToken cancellationToken)
            => await repositorioGeralSerapLegado.ObterTipoProvaLegadoPorId(request.Id);
    }
}
