using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Mapping;
using Elastic.Transport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.IoC
{
    internal static class ElasticSearchExtension
    {
        internal static void AdicionarElasticSearch(this IServiceCollection services, IConfiguration configuration)
        {
            var elasticOptions = new ElasticOptions();
            configuration.GetSection(ElasticOptions.Secao).Bind(elasticOptions, c => c.BindNonPublicProperties = true);
            services.AddSingleton(elasticOptions);

            if (elasticOptions == null) return;

            var uri = new Uri(elasticOptions.Urls.Split(',')[0].Trim());

            var settings = new ElasticsearchClientSettings(uri)
                .DefaultFieldNameInferrer(f => f.ToLowerInvariant())
                .ServerCertificateValidationCallback((_, _, _, _) => true);

            settings.DefaultIndex(elasticOptions.DefaultIndex);

            if (!string.IsNullOrEmpty(elasticOptions.Username) && !string.IsNullOrEmpty(elasticOptions.Password))
            {
                settings = settings.Authentication(new BasicAuthentication(elasticOptions.Username, elasticOptions.Password));
            }

            var client = new ElasticsearchClient(settings);

            MapearIndicesAsync(client).GetAwaiter().GetResult();

            services.AddSingleton(client);
        }

        private static async Task MapearIndicesAsync(ElasticsearchClient elasticClient)
        {
            const string indiceAlunoMatriculaTurmaDre = IndicesElastic.INDICE_ALUNO_MATRICULA_TURMA_DRE;
            const string indiceTurma = IndicesElastic.INDICE_TURMA;

            var existsResponse = await elasticClient.Indices.ExistsAsync(indiceAlunoMatriculaTurmaDre);
            if (existsResponse.Exists)
                return;

            await elasticClient.Indices.CreateAsync(indiceAlunoMatriculaTurmaDre, c => c
                .Mappings(m => m
                    .Properties<AlunoMatriculaTurmaDreDto>(p => p
                        .Text("id")
                        .IntegerNumber("codigoaluno")
                        .Text("nomealuno")
                        .Date("datanascimento", new DateProperty { Format = "MMddyyyy" })
                        .Text("nomesocialaluno")
                        .IntegerNumber("codigosituacaomatricula")
                        .Text("situacaomatricula")
                        .Date("datasituacao", new DateProperty { Format = "MMddyyyy" })
                        .LongNumber("codigomatricula")
                        .Date("datamatricula", new DateProperty { Format = "MMddyyyy" })
                    )
                )
            );

            await elasticClient.Indices.CreateAsync(indiceTurma, c => c
                .Mappings(m => m
                    .Properties<DocumentoElasticTurmaDto>(p => p
                        .Text("codigoaluno")
                        .IntegerNumber("codigoturma")
                        .Text("codigoescola")
                        .IntegerNumber("anoletivo")
                    )
                )
            );
        }
    }
}