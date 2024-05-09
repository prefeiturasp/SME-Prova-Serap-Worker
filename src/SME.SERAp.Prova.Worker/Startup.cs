using Elastic.Apm.AspNetCore;
using Elastic.Apm.DiagnosticSource;
using Elastic.Apm.SqlClient;
using Elastic.Apm.StackExchange.Redis;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using SME.SERAp.Prova.Aplicacao.Worker;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using SME.SERAp.Prova.IoC;
using StackExchange.Redis;
using System;
using System.Threading;

namespace SME.SERAp.Prova.Worker
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IHostEnvironment env;

        public Startup(IConfiguration configuration, IHostEnvironment env)
        {
            this.configuration = configuration ??
                throw new ArgumentNullException(nameof(configuration));
            this.env = env ??
                throw new ArgumentNullException(nameof(env));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            RegistraDependencias.Registrar(services, configuration);
            ConfigEnvoiromentVariables(services);
            services.AddHostedService<WorkerRabbit>();
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            var telemetriaOptions = app.ApplicationServices.GetService<TelemetriaOptions>();
            if (telemetriaOptions != null && telemetriaOptions.Apm)
            {
                app.UseElasticApm(configuration,
                   new SqlClientDiagnosticSubscriber(),
                   new HttpDiagnosticsSubscriber());

                var muxer = app.ApplicationServices.GetService<IConnectionMultiplexer>();
                muxer.UseElasticApm();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("WorkerRabbitMQ!");
            });
        }

        private void ConfigEnvoiromentVariables(IServiceCollection services)
        {
            var conexaoDadosVariaveis = new ConnectionStringOptions();

            configuration.GetSection("ConnectionStrings").Bind(conexaoDadosVariaveis, c => c.BindNonPublicProperties = true);
            services.AddSingleton(conexaoDadosVariaveis);

            var rabbitOptions = new RabbitOptions();
            configuration.GetSection("Rabbit").Bind(rabbitOptions, c => c.BindNonPublicProperties = true);
            services.AddSingleton(rabbitOptions);

            var factory = new ConnectionFactory
            {
                HostName = rabbitOptions.HostName,
                UserName = rabbitOptions.UserName,
                Password = rabbitOptions.Password,
                VirtualHost = rabbitOptions.VirtualHost
            };

            services.AddSingleton(factory);

            var conexaoRabbit = factory.CreateConnection();
            var channel = conexaoRabbit.CreateModel();

            services.AddSingleton(channel);
            services.AddSingleton(conexaoRabbit);

            var pathOptions = new PathOptions();
            configuration.GetSection("Path").Bind(pathOptions, c => c.BindNonPublicProperties = true);
            services.AddSingleton(pathOptions);

            var telemetriaOptions = new TelemetriaOptions();
            configuration.GetSection(TelemetriaOptions.Secao).Bind(telemetriaOptions, c => c.BindNonPublicProperties = true);
            services.AddSingleton(telemetriaOptions);

            var configuracaoRabbitLogOptions = new RabbitLogOptions();
            configuration.GetSection("RabbitLog").Bind(configuracaoRabbitLogOptions, c => c.BindNonPublicProperties = true);
            services.AddSingleton(configuracaoRabbitLogOptions);

            var factoryLog = new ConnectionFactory
            {
                HostName = configuracaoRabbitLogOptions.HostName,
                UserName = configuracaoRabbitLogOptions.UserName,
                Password = configuracaoRabbitLogOptions.Password,
                VirtualHost = configuracaoRabbitLogOptions.VirtualHost
            };

            var conexaoRabbitLog = factoryLog.CreateConnection();
            var channelLog = conexaoRabbitLog.CreateModel();

            var fireBaseOptions = new FireBaseOptions();
            configuration.GetSection("FireBase").Bind(fireBaseOptions, c => c.BindNonPublicProperties = true);
            services.AddSingleton(fireBaseOptions);

            var threadPoolOptions = new ThreadPoolOptions();
            configuration.GetSection("ThreadPoolOptions").Bind(threadPoolOptions, c => c.BindNonPublicProperties = true);
            if (threadPoolOptions.WorkerThreads > 0 && threadPoolOptions.CompletionPortThreads > 0)
                ThreadPool.SetMinThreads(threadPoolOptions.WorkerThreads, threadPoolOptions.CompletionPortThreads);

            var redisOptions = new RedisOptions();
            configuration.GetSection("RedisOptions").Bind(redisOptions, c => c.BindNonPublicProperties = true);

            var redisConfigurationOptions = new ConfigurationOptions()
            {
                Proxy = redisOptions.Proxy,
                SyncTimeout = redisOptions.SyncTimeout,
                EndPoints = { redisOptions.Endpoint }
            };
            var muxer = ConnectionMultiplexer.Connect(redisConfigurationOptions);
            services.AddSingleton<IConnectionMultiplexer>(muxer);

            services.AddApplicationInsightsTelemetry(configuration);
            var serviceProvider = services.BuildServiceProvider();
            var clientTelemetry = serviceProvider.GetService<TelemetryClient>();

            var servicoTelemetria = new ServicoTelemetria(clientTelemetry, telemetriaOptions);
            services.AddSingleton<IServicoTelemetria>(servicoTelemetria);
            DapperExtensionMethods.Init(servicoTelemetria);

            var apiROptions = new ApiROptions();
            configuration.GetSection(ApiROptions.Secao).Bind(apiROptions, c => c.BindNonPublicProperties = true);
            services.AddSingleton(apiROptions);
        }
    }
}