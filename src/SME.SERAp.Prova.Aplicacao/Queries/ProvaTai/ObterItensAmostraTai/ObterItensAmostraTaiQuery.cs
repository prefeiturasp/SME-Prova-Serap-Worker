using MediatR;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterItensAmostraTaiQuery : IRequest<IEnumerable<ItemAmostraTaiDto>>
    {
        public ObterItensAmostraTaiQuery(long matrizId, int tipoCurriculoGradeId)
        {
            MatrizId = matrizId;
            TipoCurriculoGradeId = tipoCurriculoGradeId;
        }

        public long MatrizId { get; }
        public int TipoCurriculoGradeId { get; }
    }
}
