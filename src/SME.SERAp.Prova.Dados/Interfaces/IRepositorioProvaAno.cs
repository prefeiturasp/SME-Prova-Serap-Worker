using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioProvaAno : IRepositorioBase<ProvaAno>
    {
        Task<IEnumerable<TipoCurriculoPeriodoAnoDto>> ObterProvaAnoPorTipoCurriculoPeriodoId(int[] tcpIds);
        Task<bool> RemoverAnosPorProvaIdAsync(long provaId);
    }
}
