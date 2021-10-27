using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunosPorTurmaCodigoQueryHandler : IRequestHandler<ObterAlunosPorTurmaCodigoQuery, IEnumerable<AlunoEolDto>>
    {
        private readonly IRepositorioAlunoEol repositorioAlunoEol;

        public ObterAlunosPorTurmaCodigoQueryHandler(IRepositorioAlunoEol repositorioAlunoEol)
        {
            this.repositorioAlunoEol = repositorioAlunoEol ?? throw new ArgumentNullException(nameof(repositorioAlunoEol));
        }
        public async Task<IEnumerable<AlunoEolDto>> Handle(ObterAlunosPorTurmaCodigoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAlunoEol.ObterAlunosPorTurmaCodigoAsync(request.TurmaCodigo);
        }
    }
}
