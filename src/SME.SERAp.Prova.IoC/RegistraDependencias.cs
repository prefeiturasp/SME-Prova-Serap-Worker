using Microsoft.Extensions.DependencyInjection;
using SME.SERAp.Prova.Aplicacao;
using SME.SERAp.Prova.Aplicacao.Interfaces;
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
            services.AddScoped<IRepositorioAlternativa, RepositorioAlternativa>();
            services.AddScoped<IRepositorioQuestao, RepositorioQuestao>();
            services.AddScoped<IRepositorioArquivo, RepositorioArquivo>();
            services.AddScoped<IRepositorioQuestaoArquivo, RepositorioQuestaoArquivo>();
            services.AddScoped<IRepositorioDre, RepositorioDre>();
            services.AddScoped<IRepositorioUe, RepositorioUe>();
            services.AddScoped<IRepositorioTurma, RepositorioTurma>();
        }

        private static void RegistrarCasosDeUso(IServiceCollection services)
        {
            services.AddScoped<ITratarProvasLegadoSyncUseCase, TratarProvasLegadoSyncUseCase>();
            services.AddScoped<ITratarProvaLegadoUseCase, TratarProvaLegadoUseCase>();
            services.AddScoped<ITratarAlternativaLegadoSyncUseCase, TratarAlternativaLegadoSyncUseCase>();
            services.AddScoped<ITratarAlternativaLegadoUseCase, TratarAlternativaLegadoLegadoUseCase>();
            services.AddScoped<ITratarQuestoesLegadoSyncUseCase, TratarQuestoesLegadoSyncUseCase>();
            services.AddScoped<ITratarQuestoesProvaLegadoUseCase, TratarQuestoesProvaLegadoUseCase>();            
            services.AddScoped<IProvaWebPushTesteUseCase, ProvaWebPushTesteUseCase>();

            // sincronização institucional 
            services.AddScoped<IExecutarSincronizacaoInstitucionalDreSyncUseCase, ExecutarSincronizacaoInstitucionalDreSyncUseCase>();
            services.AddScoped<IExecutarSincronizacaoInstitucionalDreTratarUseCase, ExecutarSincronizacaoInstitucionalDreTratarUseCase>();
            services.AddScoped<IExecutarSincronizacaoInstitucionalUeSyncUseCase, ExecutarSincronizacaoInstitucionalUeSyncUseCase>();
            services.AddScoped<IExecutarSincronizacaoInstitucionalUeTratarUseCase, ExecutarSincronizacaoInstitucionalUeTratarUseCase>();
            services.AddScoped<IExecutarSincronizacaoInstitucionalTurmaSyncUseCase, ExecutarSincronizacaoInstitucionalTurmaSyncUseCase>();
            services.AddScoped<IExecutarSincronizacaoInstitucionalTurmaTratarUseCase, ExecutarSincronizacaoInstitucionalTurmaTratarUseCase>();
        }
    }
}
