using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Dados
{
    public abstract class RepositorioElasticBase<T> : IRepositorioElasticBase<T> where T : class
    {
        private const int QuantidadeRetorno = 200;
        private const string TempoCursor = "10s";
        private const string NomeTelemetria = "Elastic";        
        
        private readonly IServicoTelemetria servicoTelemetria;
        private readonly IElasticClient elasticClient;

        protected RepositorioElasticBase(IServicoTelemetria servicoTelemetria, IElasticClient elasticClient)
        {
            this.servicoTelemetria = servicoTelemetria ?? throw new ArgumentNullException(nameof(servicoTelemetria));
            this.elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));
        }

        public async Task<IEnumerable<TResponse>> ObterListaAsync<TResponse>(string indice,
            Func<QueryContainerDescriptor<TResponse>, QueryContainer> request, string nomeConsulta,
            object parametro = null) where TResponse : class
        {
            var listaDeRetorno = new List<TResponse>();

            ISearchResponse<TResponse> response =
                await servicoTelemetria.RegistrarComRetornoAsync<ISearchResponse<TResponse>>(
                    async () => await elasticClient.SearchAsync<TResponse>(c =>
                        c.Index(indice).Query(request).Scroll(TempoCursor).Size(QuantidadeRetorno)), NomeTelemetria,
                    nomeConsulta, indice, parametro?.ToString());
            
            if (!response.IsValid)
                throw new Exception(response.ServerError?.ToString(), response.OriginalException);
            
            listaDeRetorno.AddRange(response.Documents);

            while (response.Documents.Any() && response.Documents.Count == QuantidadeRetorno)
            {
                response = await servicoTelemetria.RegistrarComRetornoAsync<ISearchResponse<TResponse>>(
                    async () => await elasticClient.ScrollAsync<TResponse>(TempoCursor, response.ScrollId),
                    NomeTelemetria, $"{nomeConsulta} scroll", indice, parametro?.ToString());

                listaDeRetorno.AddRange(response.Documents);
            }

            await elasticClient.ClearScrollAsync(new ClearScrollRequest(response.ScrollId));

            return listaDeRetorno;
        }
    }
}