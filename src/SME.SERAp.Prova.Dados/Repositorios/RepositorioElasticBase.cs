using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Dados
{
    public abstract class RepositorioElasticBase<T> : IRepositorioElasticBase<T> where T : class
    {
        private const int QuantidadeRetorno = 200;
        private const string TempoCursor = "10s";
        private const string NomeTelemetria = "Elastic";

        private readonly IServicoTelemetria servicoTelemetria;
        private readonly ElasticsearchClient elasticClient;

        protected RepositorioElasticBase(IServicoTelemetria servicoTelemetria, ElasticsearchClient elasticClient)
        {
            this.servicoTelemetria = servicoTelemetria ?? throw new ArgumentNullException(nameof(servicoTelemetria));
            this.elasticClient = elasticClient ?? throw new ArgumentNullException(nameof(elasticClient));
        }

        public async Task<T> ObterAsync(string indice, string id, string nomeConsulta, object parametro = null)
        {
            var response = await servicoTelemetria.RegistrarComRetornoAsync<GetResponse<T>>(async () =>
                await elasticClient.GetAsync<T>(id, g => g.Index(indice)), NomeTelemetria, nomeConsulta, indice, parametro?.ToString()
            );

            return response.Found == true ? response.Source : null;
        }

        public async Task<IEnumerable<TResponse>> ObterListaAsync<TResponse>(
            string indice,
            Func<QueryDescriptor<TResponse>, Query> query,
            string nomeConsulta,
            object? parametro = null) where TResponse : class
        {
            var lista = new List<TResponse>();

            var response = await servicoTelemetria.RegistrarComRetornoAsync<SearchResponse<TResponse>>(async () =>
                await elasticClient.SearchAsync<TResponse>(s => s
                    .Indices(indice)
                    .Query(q => query(q))
                    .Scroll(TempoCursor)
                    .Size(QuantidadeRetorno)),
                NomeTelemetria, nomeConsulta, indice, parametro?.ToString());

            if (response is null || !response.IsValidResponse)
                throw new Exception(response?.ElasticsearchServerError?.ToString());

            lista.AddRange(response.Documents);

            while (response.Documents.Any() && response.Documents.Count == QuantidadeRetorno)
            {
                response = await servicoTelemetria.RegistrarComRetornoAsync<SearchResponse<TResponse>>(async () =>
                    await elasticClient.ScrollAsync<TResponse>(new ScrollRequest(response.ScrollId) { Scroll = TimeSpan.FromSeconds(10) }),
                    NomeTelemetria,
                    $"{nomeConsulta} scroll",
                    indice,
                    parametro?.ToString());

                if (!response.IsValidResponse)
                    throw new Exception(response.ElasticsearchServerError?.ToString());

                lista.AddRange(response.Documents);
            }

            if (!string.IsNullOrEmpty(response.ScrollId))
            {
                await elasticClient.ClearScrollAsync(new ClearScrollRequest { ScrollId = response.ScrollId });
            }

            return lista;
        }

        public async Task<IEnumerable<TResponse>> ObterTodosAsync<TResponse>(string indice, string nomeConsulta,
            object parametro = null) where TResponse : class
        {
            var response = await servicoTelemetria.RegistrarComRetornoAsync<SearchResponse<TResponse>>(
                async () => await elasticClient.SearchAsync<TResponse>(s => s
                    .Indices(indice)
                    .Query(q => q.MatchAll())
                    .Size(QuantidadeRetorno)
                ),
                NomeTelemetria,
                nomeConsulta,
                indice,
                parametro?.ToString());

            if (!response.IsValidResponse)
                throw new Exception(response.ElasticsearchServerError?.ToString());

            return response.Documents.ToList();
        }

        public async Task<long> ObterTotalDeRegistroAsync<TDocument>(string indice, string nomeConsulta,
            object parametro = null) where TDocument : class
        {
            var response = await servicoTelemetria.RegistrarComRetornoAsync<SearchResponse<TDocument>>(
                async () => await elasticClient.SearchAsync<TDocument>(s => s
                    .Indices(indice)
                    .Query(q => q.MatchAll())
                    .Size(0) // não precisa trazer documentos
                ),
                NomeTelemetria,
                nomeConsulta,
                indice,
                parametro?.ToString());

            if (!response.IsValidResponse)
                throw new Exception(response.ElasticsearchServerError?.ToString());

            return response.Total;
        }

        public async Task<long> ObterTotalDeRegistroAPartirDeUmaCondicaoAsync<TDocument>(
            string indice,
            string nomeConsulta,
            Func<QueryDescriptor<TDocument>, Query> query,
            object parametro = null) where TDocument : class
        {
            try
            {
                var response = await servicoTelemetria.RegistrarComRetornoAsync<SearchResponse<TDocument>>(
                    async () => await elasticClient.SearchAsync<TDocument>(s => s
                        .Indices(indice)
                        .Query(q => query(q))
                        .Size(0)
                    ),
                    NomeTelemetria,
                    nomeConsulta,
                    indice,
                    parametro?.ToString());

                if (!response.IsValidResponse)
                    throw new Exception(response.ElasticsearchServerError?.ToString());

                return response.Total;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<bool> ExisteAsync(string indice, string id, string nomeConsulta, object parametro = null)
        {
            var response = await servicoTelemetria.RegistrarComRetornoAsync<GetResponse<T>>(async () =>
                await elasticClient.GetAsync<T>(id, g => g
                    .Index(indice)
                    .Source(false)
                ),
                NomeTelemetria,
                nomeConsulta,
                indice,
                parametro?.ToString());

            if (!response.IsValidResponse)
                throw new Exception(response.ElasticsearchServerError?.ToString());

            return response.Found;
        }

        public async Task InserirBulk<TRequest>(IEnumerable<TRequest> listaDeDocumentos, string indice)
            where TRequest : class
        {
            var response = await elasticClient.BulkAsync(b => b
                .Index(indice)
                .UpdateMany(listaDeDocumentos, (bu, d) => bu.Doc(d).DocAsUpsert()));

            if (!response.IsValidResponse || response.Errors)
                throw new Exception(response.ElasticsearchServerError?.ToString());
        }

        public async Task<bool> InserirAsync<TRequest>(TRequest entidade, string indice) where TRequest : class
        {
            var response = await servicoTelemetria.RegistrarComRetornoAsync<IndexResponse>(
                async () => await elasticClient.IndexAsync(entidade, d => d.Index(indice)),
                NomeTelemetria,
                $"Insert {entidade.GetType().Name}",
                indice,
                entidade.ConverterObjectParaJson());

            if (!response.IsValidResponse)
                throw new Exception(response.ElasticsearchServerError?.ToString());

            return true;
        }

        public async Task ExcluirTodos<TDocument>(string indice = "", string nomeConsulta = "") where TDocument : class
        {
            using var cancellationToken = new CancellationTokenSource(TimeSpan.FromMinutes(5));

            var response = await servicoTelemetria.RegistrarComRetornoAsync<DeleteByQueryResponse>(
                async () => await elasticClient.DeleteByQueryAsync<TDocument>(
                    d => d.Indices(indice).Query(q => q.MatchAll()), cancellationToken.Token),
                NomeTelemetria,
                nomeConsulta,
                indice);

            if (!response.IsValidResponse)
                throw new Exception(response.ElasticsearchServerError?.ToString());
        }
    }
}