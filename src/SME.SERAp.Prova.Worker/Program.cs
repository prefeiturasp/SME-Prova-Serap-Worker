using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sentry;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using SME.SERAp.Prova.IoC;
using System.Reflection;
using RabbitMQ.Client;
using StackExchange.Redis;
using SME.SERAp.Prova.Infra;
using System.Threading;

namespace SME.SERAp.Prova.Aplicacao.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(a => a.AddUserSecrets(Assembly.GetExecutingAssembly()))
                .ConfigureServices((hostContext, services) =>
                {
                    RegistraDependencias.Registrar(services);

                    services.AddHostedService<WorkerRabbit>();

                    ConfigEnvoiromentVariables(hostContext, services);

                });

        private static void ConfigEnvoiromentVariables(HostBuilderContext hostContext, IServiceCollection services)
        {
            var conexaoDadosVariaveis = new ConnectionStringOptions();
            hostContext.Configuration.GetSection("ConnectionStrings").Bind(conexaoDadosVariaveis, c => c.BindNonPublicProperties = true);
            services.AddSingleton(conexaoDadosVariaveis);

            var sentryOptions = new SentryOptions();
            hostContext.Configuration.GetSection("Sentry").Bind(sentryOptions, c => c.BindNonPublicProperties = true);
            services.AddSingleton(sentryOptions);

            var logOptions = new LogOptions();
            hostContext.Configuration.GetSection("Logs").Bind(logOptions, c => c.BindNonPublicProperties = true);
            logOptions.SentryDSN = sentryOptions.Dsn;
            services.AddSingleton(logOptions);

            var rabbitOptions = new RabbitOptions();
            hostContext.Configuration.GetSection("Rabbit").Bind(rabbitOptions, c => c.BindNonPublicProperties = true);
            services.AddSingleton(rabbitOptions);

            var pathOptions = new PathOptions();
            hostContext.Configuration.GetSection("Path").Bind(rabbitOptions, c => c.BindNonPublicProperties = true);
            services.AddSingleton(pathOptions);

            var factory = new ConnectionFactory
            {
                HostName = rabbitOptions.HostName,
                UserName = rabbitOptions.UserName,
                Password = rabbitOptions.Password,
                VirtualHost = rabbitOptions.VirtualHost
            };
            
            services.AddSingleton(factory);

            var conexaoRabbit = factory.CreateConnection();
            IModel channel = conexaoRabbit.CreateModel();
            
            services.AddSingleton(channel);
            services.AddSingleton(conexaoRabbit);

            var fireBaseOptions = new FireBaseOptions();
            hostContext.Configuration.GetSection("FireBase").Bind(fireBaseOptions, c => c.BindNonPublicProperties = true);
            services.AddSingleton(fireBaseOptions);

            var threadPoolOptions = new ThreadPoolOptions();
            hostContext.Configuration.GetSection("ThreadPoolOptions").Bind(threadPoolOptions, c => c.BindNonPublicProperties = true);
            if (threadPoolOptions.WorkerThreads > 0 && threadPoolOptions.CompletionPortThreads > 0)
                ThreadPool.SetMinThreads(threadPoolOptions.WorkerThreads, threadPoolOptions.CompletionPortThreads);

            var redisOptions = new RedisOptions();
            hostContext.Configuration.GetSection("RedisOptions").Bind(redisOptions, c => c.BindNonPublicProperties = true);
            var redisConfigurationOptions = new ConfigurationOptions()
            {
                Proxy = redisOptions.Proxy,
                SyncTimeout = redisOptions.SyncTimeout
            };
            redisOptions.Endpoints.ForEach(endpoint => redisConfigurationOptions.EndPoints.Add(endpoint));
            var muxer = ConnectionMultiplexer.Connect(redisConfigurationOptions);
            services.AddSingleton<IConnectionMultiplexer>(muxer);
        }
    }
}
