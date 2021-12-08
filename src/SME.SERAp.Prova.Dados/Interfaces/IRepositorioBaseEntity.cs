using SME.SERAp.Prova.Dominio;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioBaseEntity<T> where T : EntidadeBase
    {
        Task InserirVariosAsync(IEnumerable<T> entidades);
        Task AlterarVariosAsync(IEnumerable<T> entidades);
    }
}
