using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunosEolPorTurmasCodigoQueryHandler : IRequestHandler<ObterAlunosEolPorTurmasCodigoQuery, IEnumerable<AlunoEolDto>>
    {
        private readonly IRepositorioAlunoEol repositorioAlunoEol;

        public ObterAlunosEolPorTurmasCodigoQueryHandler(IRepositorioAlunoEol repositorioAlunoEol)
        {
            this.repositorioAlunoEol = repositorioAlunoEol ?? throw new ArgumentNullException(nameof(repositorioAlunoEol));
        }
        public async Task<IEnumerable<AlunoEolDto>> Handle(ObterAlunosEolPorTurmasCodigoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAlunoEol.ObterAlunosPorTurmasCodigoAsync(request.TurmasCodigo);
        }
    }
}
