using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioProvaAno : IRepositorioBase<ProvaAno>
    {
        Task<IEnumerable<TipoCurriculoPeriodoAnoDto>> ObterProvaAnoPorTipoCurriculoPeriodoId(int[] tcpIds);
        Task<bool> RemoverAnosPorProvaIdAsync(long provaId);
    }
}
