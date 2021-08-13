using System.Threading.Tasks;
using SME.SERAp.Prova.Dominio.Dtos;

namespace SME.SERAp.Prova.Dados.Interfaces
{
    public interface IRepositorioProvaLegado
    {
        Task<ObterIdsProvaLegadoDto> ObterIds();
    }
}