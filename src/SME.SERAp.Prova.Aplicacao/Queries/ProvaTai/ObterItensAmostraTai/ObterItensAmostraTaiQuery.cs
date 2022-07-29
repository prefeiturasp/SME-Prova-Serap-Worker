using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterItensAmostraTaiQuery : IRequest<IEnumerable<ItemAmostraTaiDto>>
    {
        public ObterItensAmostraTaiQuery(long matrizId, int[] tipoCurriculoGradeIds)
        {
            MatrizId = matrizId;
            TipoCurriculoGradeIds = tipoCurriculoGradeIds;
        }

        public long MatrizId { get; set; }
        public int[] TipoCurriculoGradeIds { get; set; }
    }
}
