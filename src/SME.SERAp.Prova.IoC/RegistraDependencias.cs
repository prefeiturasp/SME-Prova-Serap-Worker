using Microsoft.Extensions.DependencyInjection;
using SME.SERAp.Prova.Aplicacao;
using SME.SERAp.Prova.Dados;

namespace SME.SERAp.Prova.IoC
{
    public static class RegistraDependencias
    {
        public static void Registrar(IServiceCollection services)
        {
            services.AdicionarMediatr();
            services.AdicionarValidadoresFluentValidation();

            RegistrarRepositorios(services);            
            RegistrarComandos(services);
            RegistrarConsultas(services);            
            RegistrarCasosDeUso(services);
            RegistrarMapeamentos.Registrar();
        }

        private static void RegistrarComandos(IServiceCollection services)
        {

        }

        private static void RegistrarConsultas(IServiceCollection services)
        {

        }

        private static void RegistrarRepositorios(IServiceCollection services)
        {

        }

        private static void RegistrarCasosDeUso(IServiceCollection services)
        {
            services.AddTransient<ITesteRabbitUseCase, TesteRabbitUseCase>();
        }
    }
}
