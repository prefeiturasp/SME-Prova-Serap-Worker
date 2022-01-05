using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioProvaAdesaoLegado
    {
        Task<IEnumerable<ProvaAdesaoEntityDto>> ObterAdesaoPorProvaId(long provaId);
    }
}
