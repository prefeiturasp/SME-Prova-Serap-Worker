using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sentry;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using SME.SERAp.Prova.IoC;
using System.Reflection;
using RabbitMQ.Client;

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

            var rabbitOptions = new RabbitOptions();
            hostContext.Configuration.GetSection("Rabbit").Bind(rabbitOptions, c => c.BindNonPublicProperties = true);
            services.AddSingleton(rabbitOptions);
     
            var factory = new ConnectionFactory
            {
                HostName = rabbitOptions.HostName,
                UserName = rabbitOptions.UserName,
                Password = rabbitOptions.Password,
                VirtualHost = rabbitOptions.VirtualHost
            };
            
            services.AddSingleton(factory);
 
        }
    }
}
