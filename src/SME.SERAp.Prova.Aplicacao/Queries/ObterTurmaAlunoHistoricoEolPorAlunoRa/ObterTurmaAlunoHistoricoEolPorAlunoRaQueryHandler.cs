using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmaAlunoHistoricoEolPorAlunosRaQueryHandler : IRequestHandler<ObterTurmaAlunoHistoricoEolPorAlunosRaQuery, IEnumerable<TurmaEolDto>>
    {

        private readonly IRepositorioTurmaEol repositorioTurmaEol;

        public ObterTurmaAlunoHistoricoEolPorAlunosRaQueryHandler(IRepositorioTurmaEol repositorioTurmaEol)
        {
            this.repositorioTurmaEol = repositorioTurmaEol ?? throw new ArgumentNullException(nameof(repositorioTurmaEol));
        }

        public async Task<IEnumerable<TurmaEolDto>> Handle(ObterTurmaAlunoHistoricoEolPorAlunosRaQuery request, CancellationToken cancellationToken)
            => await repositorioTurmaEol.ObterTurmasAlunoHistoricoPorAlunosRa(request.AlunoRa);
    }
}
