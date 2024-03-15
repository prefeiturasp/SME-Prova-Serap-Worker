using MediatR;
using SME.SERAp.Prova.Aplicacao.Queries.ObterAlunosResultadoProvaAdesao;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SME.SERAp.Prova.Aplicacao.Queries.ObterAlunosResultadoProvaDeficiencia
{
    public class ObterAlunosResultadoProvaDeficienciaQueryHandler : IRequestHandler<ObterAlunosResultadoProvaDeficienciaQuery, IEnumerable<ConsolidadoProvaRespostaDto>>
    {

        private readonly IRepositorioProva repositorioProva;

        public ObterAlunosResultadoProvaDeficienciaQueryHandler(IRepositorioProva repositorioProva)
        {
            this.repositorioProva = repositorioProva ?? throw new ArgumentNullException(nameof(repositorioProva));
        }

        public async Task<IEnumerable<ConsolidadoProvaRespostaDto>> Handle(ObterAlunosResultadoProvaDeficienciaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioProva.ObterAlunosProvaDeficienciaPorProvaLegadoIdETurmasCodigos(request.ProvaId, request.TurmasCodigos);
        }
    }
}


