using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.Queries.ObterProvaLegadoItemFormatoTai
{
    public class ObterProvaLegadoItemFormatoTaiQueryHandler : IRequestHandler<ObterProvaLegadoItemFormatoTaiQuery, ProvaFormatoTaiItem>
    {
        private readonly IRepositorioProvaLegado repositorioProvaLegado;

        public ObterProvaLegadoItemFormatoTaiQueryHandler(IRepositorioProvaLegado repositorioProvaLegado)
        {
            this.repositorioProvaLegado = repositorioProvaLegado ?? throw new ArgumentNullException(nameof(repositorioProvaLegado));
        }

        public Task<ProvaFormatoTaiItem> Handle(ObterProvaLegadoItemFormatoTaiQuery request, CancellationToken cancellationToken)
        {
            return repositorioProvaLegado.ObterFormatoTaiItemPorId(request.ProvaLegadoId);
        }
    }
}
