using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        public async Task<T> ObterAsync(string indice, string id, string nomeConsulta, object parametro = null)
        {
            GetResponse<T> response = await servicoTelemetria.RegistrarComRetornoAsync<GetResponse<T>>(async () => 
                    await elasticClient.GetAsync(DocumentPath<T>.Id(id).Index(indice)),
                NomeTelemetria,
                nomeConsulta,
                indice,
                parametro?.ToString());

            return response.IsValid ? response.Source : null;
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

        public async Task<IEnumerable<TResponse>> ObterTodosAsync<TResponse>(string indice, string nomeConsulta,
            object parametro = null) where TResponse : class
        {
            var search = new SearchDescriptor<T>(indice).MatchAll();

            ISearchResponse<TResponse> response = await servicoTelemetria.RegistrarComRetornoAsync<ISearchResponse<T>>(
                async () => await elasticClient.SearchAsync<TResponse>(search),
                NomeTelemetria,
                nomeConsulta,
                indice,
                parametro?.ToString());

            if (!response.IsValid)
                throw new Exception(response.ServerError?.ToString(), response.OriginalException);

            return response.Hits.Select(hit => hit.Source).ToList();
        }

        public async Task<long> ObterTotalDeRegistroAsync<TDocument>(string indice, string nomeConsulta,
            object parametro = null) where TDocument : class
        {
            var search = new SearchDescriptor<TDocument>(indice).MatchAll();
            ISearchResponse<TDocument> response = await servicoTelemetria.RegistrarComRetornoAsync<ISearchResponse<T>>(
                async () => await elasticClient.SearchAsync<TDocument>(search),
                NomeTelemetria,
                nomeConsulta,
                indice,
                parametro?.ToString());

            if (!response.IsValid)
                throw new Exception(response.ServerError?.ToString(), response.OriginalException);

            return response.Total;
        }

        public async Task<long> ObterTotalDeRegistroAPartirDeUmaCondicaoAsync<TDocument>(string indice,
            string nomeConsulta, Func<QueryContainerDescriptor<TDocument>, QueryContainer> request,
            object parametro = null) where TDocument : class
        {
            try
            {
                ISearchResponse<T> response =
                    await servicoTelemetria.RegistrarComRetornoAsync<ISearchResponse<TDocument>>(async () =>
                            await elasticClient.SearchAsync<TDocument>(s => s.Index(indice)
                                .Query(request)
                                .Scroll(TempoCursor)
                                .Size(QuantidadeRetorno)),
                        NomeTelemetria,
                        nomeConsulta,
                        indice,
                        parametro?.ToString());

                if (!response.IsValid)
                    throw new Exception(response.ServerError?.ToString(), response.OriginalException);

                return response.Total;
            }
            catch(Exception ex)
            {
                return 0;
            }
        }

        public async Task<bool> ExisteAsync(string indice, string id, string nomeConsulta, object parametro = null)
        {
            ExistsResponse response = await servicoTelemetria.RegistrarComRetornoAsync<ExistsResponse>(async () =>
                    await elasticClient.DocumentExistsAsync(DocumentPath<T>.Id(id).Index(indice)),
                NomeTelemetria,
                nomeConsulta,
                indice,
                parametro?.ToString());

            if (!response.IsValid)
                throw new Exception(response.ServerError?.ToString(), response.OriginalException);

            return response.Exists;
        }

        public async Task InserirBulk<TRequest>(IEnumerable<TRequest> listaDeDocumentos, string indice)
            where TRequest : class
        {
            var response = await elasticClient.BulkAsync(b => b
                .Index(indice)
                .UpdateMany(listaDeDocumentos, (bu, d) => bu.Doc(d).DocAsUpsert()));

            if (!response.IsValid && response.Errors)
                throw new Exception(response.ServerError?.ToString(), response.OriginalException);
        }

        public async Task<bool> InserirAsync<TRequest>(TRequest entidade, string indice) where TRequest : class
        {
            var response = await servicoTelemetria.RegistrarComRetornoAsync<ISearchResponse<T>>(async () =>
                    await elasticClient.IndexAsync(entidade, descriptor => descriptor.Index(indice)),
                NomeTelemetria,
                $"Insert {entidade.GetType().Name}",
                indice,
                entidade.ConverterObjectParaJson());

            if (!response.IsValid)
                throw new Exception(response.ServerError?.ToString(), response.OriginalException);

            return true;
        }

        public async Task ExcluirTodos<TDocument>(string indice = "", string nomeConsulta = "") where TDocument : class
        {
            var cancelationToken = new CancellationTokenSource(TimeSpan.FromMinutes(5));

            DeleteByQueryResponse response = await servicoTelemetria.RegistrarComRetornoAsync<DeleteByQueryResponse>(
                async () => await elasticClient.DeleteByQueryAsync<TDocument>(
                    q => q.Index(indice).Query(rq => rq.MatchAll()), cancelationToken.Token),
                "Elastic",
                nomeConsulta,
                indice);

            if (!response.IsValid)
                throw new Exception(response.ServerError?.ToString(), response.OriginalException);
        }
    }
}