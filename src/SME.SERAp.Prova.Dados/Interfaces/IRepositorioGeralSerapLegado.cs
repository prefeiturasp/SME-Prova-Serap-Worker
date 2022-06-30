using SME.SERAp.Prova.Dominio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioGeralSerapLegado
    {
        Task<TipoProva> ObterTipoProvaLegadoPorId(long tipoProvaLegadoId);
        Task<IEnumerable<Guid>> ObterTipoProvaDeficienciaPorTipoProvaLegadoId(long tipoProvaLegadoId);
        Task<AreaConhecimentoSerap> ObterAreaConhecimentoSerapPorDisciplinaId(long disciplinaId);
    }
}
