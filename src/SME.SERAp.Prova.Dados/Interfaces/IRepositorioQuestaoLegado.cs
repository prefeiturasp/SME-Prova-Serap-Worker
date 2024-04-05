using SME.SERAp.Prova.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioQuestaoLegado
    {
        Task<IEnumerable<ItemAmostraTaiDto>> ObterItensAmostraTai(long matrizId, int tipoCurriculoGradeId);
    }
}
