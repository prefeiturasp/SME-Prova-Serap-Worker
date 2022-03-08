using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmaAlunoHistoricoEolPorAlunoRaQueryHandler : IRequestHandler<ObterTurmaAlunoHistoricoEolPorAlunoRaQuery, IEnumerable<TurmaEolDto>>
    {

        private readonly IRepositorioTurmaEol repositorioTurmaEol;

        public ObterTurmaAlunoHistoricoEolPorAlunoRaQueryHandler(IRepositorioTurmaEol repositorioTurmaEol)
        {
            this.repositorioTurmaEol = repositorioTurmaEol ?? throw new ArgumentNullException(nameof(repositorioTurmaEol));
        }

        public async Task<IEnumerable<TurmaEolDto>> Handle(ObterTurmaAlunoHistoricoEolPorAlunoRaQuery request, CancellationToken cancellationToken)
            => await repositorioTurmaEol.ObterTurmasAlunoHistoricoPorAlunoRa(request.AlunoRa);
    }
}
