using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterExtracaoProvaRespostaQueryHandler :
        IRequestHandler<ObterExtracaoProvaRespostaQuery, IEnumerable<ConsolidadoProvaRespostaDto>>
    {
        private readonly IRepositorioResultadoProvaConsolidado repositorioResultadoProvaConsolidado;

        public ObterExtracaoProvaRespostaQueryHandler(IRepositorioResultadoProvaConsolidado repositorioResultadoProvaConsolidado)
        {
            this.repositorioResultadoProvaConsolidado = repositorioResultadoProvaConsolidado ??
                                          throw new ArgumentNullException(nameof(repositorioResultadoProvaConsolidado));
        }

        public async Task<IEnumerable<ConsolidadoProvaRespostaDto>> Handle(ObterExtracaoProvaRespostaQuery request,
            CancellationToken cancellationToken)
            => await repositorioResultadoProvaConsolidado.ObterExtracaoProvaRespostaQuery(request.ProvaSerapId, request.DreCodigoEol, request.UeCodigoEol, request.TurmasCodigosEol);
    }
}