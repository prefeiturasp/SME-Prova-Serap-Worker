using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SME.SERAp.Prova.Aplicacao;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dados.Repositorios;
using SME.SERAp.Prova.Dados.SerapLegado;

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
            services.AddScoped<IRepositorioProvaLegado, RepositorioProvaLegado>();
        }

        private static void RegistrarCasosDeUso(IServiceCollection services)
        {
            services.AddTransient<ITesteRabbitUseCase, TesteRabbitUseCase>();
            services.AddScoped<IObterIdsProvaLegadoSyncUseCase, ObterIdsProvaLegadoSyncUseCase>();
            services.AddScoped<ITratarProvaLegadoLegadoUseCase, TratarProvaLegadoLegadoUseCase>();
        }
    }
}
