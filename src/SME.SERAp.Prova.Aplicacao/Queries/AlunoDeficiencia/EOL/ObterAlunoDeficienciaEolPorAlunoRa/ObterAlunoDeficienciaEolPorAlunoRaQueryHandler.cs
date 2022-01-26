using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunoDeficienciaEolPorAlunoRaQueryHandler : IRequestHandler<ObterAlunoDeficienciaEolPorAlunoRaQuery, IEnumerable<int>>
    {

        private readonly IRepositorioAlunoEol repositorioAlunoEol;

        public ObterAlunoDeficienciaEolPorAlunoRaQueryHandler(IRepositorioAlunoEol repositorioAlunoEol)
        {
            this.repositorioAlunoEol = repositorioAlunoEol ?? throw new ArgumentNullException(nameof(repositorioAlunoEol));
        }

        public async Task<IEnumerable<int>> Handle(ObterAlunoDeficienciaEolPorAlunoRaQuery request,
            CancellationToken cancellationToken)
            => await repositorioAlunoEol.ObterAlunoDeficienciaPorAlunoRa(request.AlunoRa);
    }
}
