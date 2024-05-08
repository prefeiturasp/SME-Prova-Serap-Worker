using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nest;

namespace SME.SERAp.Prova.Dados
{
    public interface IRepositorioElasticBase<T> where T: class
    {
        Task<T> ObterAsync(string indice, string id, string nomeConsulta, object parametro = null);

        Task<IEnumerable<TResponse>> ObterListaAsync<TResponse>(string indice,
            Func<QueryContainerDescriptor<TResponse>, QueryContainer> request, string nomeConsulta,
            object parametro = null) where TResponse : class;

        Task<IEnumerable<TResponse>> ObterTodosAsync<TResponse>(string indice, string nomeConsulta,
            object parametro = null) where TResponse : class;

        Task<long> ObterTotalDeRegistroAsync<TDocument>(string indice, string nomeConsulta, object parametro = null)
            where TDocument : class;

        Task<long> ObterTotalDeRegistroAPartirDeUmaCondicaoAsync<TDocument>(string indice, string nomeConsulta,
            Func<QueryContainerDescriptor<TDocument>, QueryContainer> request, object parametro = null)
            where TDocument : class;
        
        Task<bool> ExisteAsync(string indice, string id, string nomeConsulta, object parametro = null);
        Task InserirBulk<TRequest>(IEnumerable<TRequest> listaDeDocumentos, string indice) where TRequest : class;
        Task<bool> InserirAsync<TRequest>(TRequest entidade, string indice) where TRequest : class;
        Task ExcluirTodos<TDocument>(string indice = "", string nomeConsulta = "") where TDocument : class;
    }
}