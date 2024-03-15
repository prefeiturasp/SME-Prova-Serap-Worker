using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunosResultadoProvaDeficienciaQueryHandler : IRequestHandler<ObterAlunosResultadoProvaDeficienciaQuery, IEnumerable<ConsolidadoAlunoProvaDto>>
    {

        private readonly IRepositorioProva repositorioProva;

        public ObterAlunosResultadoProvaDeficienciaQueryHandler(IRepositorioProva repositorioProva)
        {
            this.repositorioProva = repositorioProva ?? throw new ArgumentNullException(nameof(repositorioProva));
        }

        public async Task<IEnumerable<ConsolidadoAlunoProvaDto>> Handle(ObterAlunosResultadoProvaDeficienciaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioProva.ObterAlunosProvaDeficienciaPorProvaLegadoIdETurmasCodigos(request.ProvaId, request.TurmasCodigos);
        }
    }
}


