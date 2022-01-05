using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterDadosAlunosParaAdesaoPorRaQueryHandler : IRequestHandler<ObterDadosAlunosParaAdesaoPorRaQuery, IEnumerable<ProvaAdesao>>
    {
        private readonly IRepositorioProvaAdesao repositorioProvaAdesao;

        public ObterDadosAlunosParaAdesaoPorRaQueryHandler(IRepositorioProvaAdesao repositorioProvaAdesao)
        {
            this.repositorioProvaAdesao = repositorioProvaAdesao ?? throw new ArgumentNullException(nameof(repositorioProvaAdesao));
        }

        public async Task<IEnumerable<ProvaAdesao>> Handle(ObterDadosAlunosParaAdesaoPorRaQuery request, CancellationToken cancellationToken)
            => await repositorioProvaAdesao.ObterDadosAlunosParaAdesaoPorRa(request.AlunosRa);
    }
}
