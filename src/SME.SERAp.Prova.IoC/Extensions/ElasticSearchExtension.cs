using System;
using System.Collections.Generic;
using System.Linq;
using Elasticsearch.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nest;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;

namespace SME.SERAp.Prova.IoC
{
    internal static class ElasticSearchExtension
    {
        internal static void AdicionarElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            RegistrarElasticOptions(services, configuration);
            
            var serviceProvider = services.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<ElasticOptions>>();
            var elasticOptions = options?.Value;
            if (elasticOptions == null)
                return;

            var nodes = new List<Uri>();
            if (elasticOptions.Urls.Contains(','))
            {
                var urls = elasticOptions.Urls.Split(',');
                nodes.AddRange(urls.Select(url => new Uri(url)));
            }
            else
                nodes.Add(new Uri(elasticOptions.Urls));

            var connectionPool = new StaticConnectionPool(nodes);
            var connectionStrings =
                new ConnectionSettings(connectionPool).ServerCertificateValidationCallback(
                    (sender, cert, chain, errors) => true);

            if (!string.IsNullOrEmpty(elasticOptions.Username) && !string.IsNullOrEmpty(elasticOptions.Password))
                connectionStrings.BasicAuthentication(elasticOptions.Username, elasticOptions.Password);

            var elasticClient = new ElasticClient(connectionStrings);
            
            MapearIndices(elasticClient);
            
            services.AddSingleton<IElasticClient>(elasticClient);            
        }

        private static void RegistrarElasticOptions(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions<ElasticOptions>().Bind(configuration.GetSection(ElasticOptions.Secao),
                c => c.BindNonPublicProperties = true);
            services.AddSingleton<ElasticOptions>();            
        }

        private static void MapearIndices(IElasticClient elasticClient)
        {
            elasticClient
                .Map<AlunoMatriculaTurmaDreDto>(map =>
                    map.Index(Indices.Index(IndicesElastic.INDICE_ALUNO_MATRICULA_TURMA_DRE)).AutoMap());
        }
    }
}