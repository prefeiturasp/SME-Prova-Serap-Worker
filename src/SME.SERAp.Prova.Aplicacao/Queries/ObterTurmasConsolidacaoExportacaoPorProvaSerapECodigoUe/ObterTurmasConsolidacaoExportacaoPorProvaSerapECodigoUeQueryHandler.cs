using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;


namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmasConsolidacaoExportacaoPorProvaSerapECodigoUeQueryHandler : IRequestHandler<ObterTurmasConsolidacaoExportacaoPorProvaSerapECodigoUeQuery, IEnumerable<Turma>>
    {

        private readonly IRepositorioTurma repositorioTurma;

        public ObterTurmasConsolidacaoExportacaoPorProvaSerapECodigoUeQueryHandler(IRepositorioTurma repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<IEnumerable<Turma>> Handle(ObterTurmasConsolidacaoExportacaoPorProvaSerapECodigoUeQuery request, CancellationToken cancellationToken)
            => await repositorioTurma.ObterTurmasConsolidacaoExportacaoPorProvaSerapECodigoUe(request.ProvaSerap, request.CodigoUe);
    }
}
