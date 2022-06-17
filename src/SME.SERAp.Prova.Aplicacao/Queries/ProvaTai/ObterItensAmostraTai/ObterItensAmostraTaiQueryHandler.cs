using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterItensAmostraTaiQueryHandler : IRequestHandler<ObterItensAmostraTaiQuery, IEnumerable<ItemAmostraTaiDto>>
    {

        private readonly IRepositorioQuestaoLegado repositorioQuestaoLegado;

        public ObterItensAmostraTaiQueryHandler(IRepositorioQuestaoLegado repositorioQuestaoLegado)
        {
            this.repositorioQuestaoLegado = repositorioQuestaoLegado ?? throw new ArgumentNullException(nameof(repositorioQuestaoLegado));
        }

        public async Task<IEnumerable<ItemAmostraTaiDto>> Handle(ObterItensAmostraTaiQuery request, CancellationToken cancellationToken)
        {
            return await repositorioQuestaoLegado.ObterItensAmostraTai(request.MatrizId, request.TipoCurriculoGradeIds);
        }
    }
}
