﻿using Microsoft.Extensions.DependencyInjection;
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
            RegistrarCasosDeUso(services);
            RegistrarMapeamentos.Registrar();
        }

        private static void RegistrarRepositorios(IServiceCollection services)
        {
            services.AddScoped<IRepositorioProvaLegado, RepositorioProvaLegado>();
            services.AddScoped<IRepositorioProva, RepositorioProva>();
            services.AddScoped<IRepositorioExecucaoControle, RepositorioExecucaoControle>();
            services.AddScoped<IRepositorioProvaAno, RepositorioProvaAno>();
        }

        private static void RegistrarCasosDeUso(IServiceCollection services)
        {
            services.AddScoped<ITratarProvasLegadoSyncUseCase, TratarProvasLegadoSyncUseCase>();
            services.AddScoped<ITratarProvaLegadoLegadoUseCase, TratarProvaLegadoUseCase>();
        }
    }
}
