using System;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioCache
    {
        Task RemoverRedisAsync(string nomeChave);
    }
}
