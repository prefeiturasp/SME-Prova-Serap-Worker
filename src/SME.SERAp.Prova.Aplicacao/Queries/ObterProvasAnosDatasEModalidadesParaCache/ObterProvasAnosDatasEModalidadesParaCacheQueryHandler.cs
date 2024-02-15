using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterProvasAnosDatasEModalidadesParaCacheQueryHandler : IRequestHandler<ObterProvasAnosDatasEModalidadesParaCacheQuery, IEnumerable<ProvaAnoDto>>
    {
        private readonly IRepositorioProvaAno repositorioProvaAno;

        public ObterProvasAnosDatasEModalidadesParaCacheQueryHandler(IRepositorioProvaAno repositorioProvaAno)
        {
            this.repositorioProvaAno = repositorioProvaAno ?? throw new ArgumentNullException(nameof(repositorioProvaAno));
        }

        public async Task<IEnumerable<ProvaAnoDto>> Handle(ObterProvasAnosDatasEModalidadesParaCacheQuery request, CancellationToken cancellationToken)
        {
            return await repositorioProvaAno.ObterProvasAnosDatasEModalidadesParaCacheAsync();
        }
    }
}