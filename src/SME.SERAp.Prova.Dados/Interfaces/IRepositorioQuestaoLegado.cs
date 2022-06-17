using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioQuestaoLegado
    {
        Task<IEnumerable<ItemAmostraTaiDto>> ObterItensAmostraTai(long matrizId, int[] tipoCurriculoGradeIds);
    }
}
