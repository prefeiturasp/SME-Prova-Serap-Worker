using SME.SERAp.Prova.Dominio;
using System.Threading.Tasks;
using System;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioTipoDeficiencia : IRepositorioBase<TipoDeficiencia>
    {
        Task<TipoDeficiencia> ObterPorLegadoId(Guid legadoId);
        Task<TipoDeficiencia> ObterPorCodigoEol(int codigoEol);
    }
}
