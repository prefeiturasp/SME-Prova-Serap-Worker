using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nest;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioElasticBase<T> where T: class
    {
        Task<IEnumerable<TResponse>> ObterListaAsync<TResponse>(string indice,
            Func<QueryContainerDescriptor<TResponse>, QueryContainer> request, string nomeConsulta,
            object parametro = null) where TResponse : class;
    }
}