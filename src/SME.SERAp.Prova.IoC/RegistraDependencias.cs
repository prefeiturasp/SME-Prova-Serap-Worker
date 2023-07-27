using Microsoft.Extensions.DependencyInjection;
using SME.SERAp.Prova.Aplicacao;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Aplicacao.UseCase;
using SME.SERAp.Prova.Aplicacao.UseCase.ProvaSaoPaulo.Participacao.CicloDre;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dados.Cache;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dados.Repositorios;
using SME.SERAp.Prova.Dados.Repositorios.ProvaSP;
using SME.SERAp.Prova.Dados.Repositorios.Serap;
using SME.SERAp.Prova.Infra.Interfaces;
using SME.SERAp.Prova.Infra.Services;

namespace SME.SERAp.Prova.IoC
{
    public static class RegistraDependencias
    {
        public static void Registrar(IServiceCollection services)
        {
            services.AdicionarMediatr();
            services.AdicionarValidadoresFluentValidation();
            services.AddPolicies();
            RegistrarRepositorios(services);
            RegistrarServicos(services);
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
            services.AddScoped<IRepositorioAlternativaArquivo, RepositorioAlternativaArquivo>();
            services.AddScoped<IRepositorioQuestaoArquivo, RepositorioQuestaoArquivo>();
            services.AddScoped<IRepositorioDre, RepositorioDre>();
            services.AddScoped<IRepositorioUe, RepositorioUe>();
            services.AddScoped<IRepositorioTurma, RepositorioTurma>();
            services.AddScoped<IRepositorioAluno, RepositorioAluno>();
            services.AddScoped<IRepositorioAlunoEol, RepositorioAlunoEol>();
            services.AddScoped<IRepositorioCadernoAluno, RepositorioCadernoAluno>();
            services.AddScoped<IRepositorioQuestaoAlunoResposta, RepositorioQuestaoAlunoResposta>();            
            services.AddScoped<IRepositorioContextoProva, RepositorioContextoProva>();
            services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
            services.AddScoped<IRepositorioPreferenciasUsuario, RepositorioPreferenciasUsuario>();
            services.AddScoped<IRepositorioCache, RepositorioCache>();
            services.AddScoped<IRepositorioProvaAluno, RepositorioProvaAluno>();
            services.AddScoped<IRepositorioDreEntity, RepositorioDreEntity>();
            services.AddScoped<IRepositorioUeEntity, RepositorioUeEntity>();
            services.AddScoped<IRepositorioTurmaEntity, RepositorioTurmaEntity>();
            services.AddScoped<IRepositorioAlunoEntity, RepositorioAlunoEntity>();
            services.AddScoped<IRepositorioFrequenciaAlunoSgp, RepositorioFrequenciaAlunoSgp>();
            services.AddScoped<IRepositorioExportacaoResultado, RepositorioExportacaoResultado>();
            services.AddScoped<IRepositorioResultadoProvaConsolidado, RepositorioResultadoProvaConsolidado>();
            services.AddScoped<IRepositorioResultadoProvaConsolidadoEntity, RepositorioResultadoProvaConsolidadoEntity>();
            services.AddScoped<IRepositorioExportacaoResultadoItem, RepositorioExportacaoResultadoItem>();
            services.AddScoped<IRepositorioParametroSistema, RepositorioParametroSistema>();
            services.AddScoped<IRepositorioProvaAdesao, RepositorioProvaAdesao>();
            services.AddScoped<IRepositorioProvaAdesaoEntity, RepositorioProvaAdesaoEntity>();
            services.AddScoped<IRepositorioProvaAdesaoLegado, RepositorioProvaAdesaoLegado>();
            services.AddScoped<IRepositorioQuestaoAudio, RepositorioQuestaoAudio>();
            services.AddScoped<IRepositorioTipoProva, RepositorioTipoProva>();
            services.AddScoped<IRepositorioGeralSerapLegado, RepositorioGeralSerapLegado>();
            services.AddScoped<IRepositorioTipoDeficiencia, RepositorioTipoDeficiencia>();
            services.AddScoped<IRepositorioTipoProvaDeficiencia, RepositorioTipoProvaDeficiencia>();
            services.AddScoped<IRepositorioAlunoDeficiencia, RepositorioAlunoDeficiencia>();
            services.AddScoped<IRepositorioQuestaoVideo, RepositorioQuestaoVideo>();
            services.AddScoped<IRepositorioTurmaEol, RepositorioTurmaEol>();
            services.AddScoped<IRepositorioTurmaAlunoHistorico, RepositorioTurmaAlunoHistorico>();
            services.AddScoped<IRepositorioGrupoSerapCoreSso, RepositorioGrupoSerapCoreSso>();
            services.AddScoped<IRepositorioUsuarioSerapCoreSso, RepositorioUsuarioSerapCoreSso>();
            services.AddScoped<IRepositorioUsuarioGrupoSerapCoreSso, RepositorioUsuarioGrupoSerapCoreSso>();
            services.AddScoped<IRepositorioUsuarioCoreSso, RepositorioUsuarioCoreSso>();
            services.AddScoped<IRepositorioGeralCoreSso, RepositorioGeralCoreSso>();
            services.AddScoped<IRepositorioGeralEol, RepositorioGeralEol>();
            services.AddScoped<IRepositorioAbrangencia, RepositorioAbrangencia>();
            services.AddScoped<IRepositorioQuestaoCompleta, RepositorioQuestaoCompleta>();
            services.AddScoped<IRepositorioDownloadProvaAluno, RepositorioDownloadProvaAluno>();
            services.AddScoped<IRepositorioAlunoProvaProficiencia, RepositorioAlunoProvaProficiencia>();
            services.AddScoped<IRepositorioProficienciaProvaSP, RepositorioProficienciaProvaSP>();
            services.AddScoped<IRepositorioQuestaoLegado, RepositorioQuestaoLegado>();
            services.AddScoped<IRepositorioVersaoAppDispositivo, RepositorioVersaoAppDispositivo>();
            services.AddScoped<IRepositorioUsuarioDispositivo, RepositorioUsuarioDispositivo>();
            services.AddScoped<IRepositorioProvaAlunoReabertura, RepositorioProvaReabertura>();
            services.AddScoped<IRepositorioProvaGrupoPermissaoEntity, RepositorioProvaGrupoPermissaoEntity>();
            services.AddScoped<IRepositorioProvaGrupoPermissao, RepositorioProvaGrupoPermissao>();
            services.AddScoped<IRepositorioQuestaoTri, RepositorioQuestaoTri>();
            services.AddScoped<IRepositorioArquivoResultadoPsp, RepositorioArquivoResultadoPsp>();
            services.AddScoped<IRepositorioResultadoAluno, RepositorioResultadoAluno>();
            services.AddScoped<IRepositorioResultadoSme, RepositorioResultadoSme>();
            services.AddScoped<IRepositorioResultadoDre, RepositorioResultadoDre>();
            services.AddScoped<IRepositorioResultadoEscola, RepositorioResultadoEscola>();
            services.AddScoped<IRepositorioResultadoTurma, RepositorioResultadoTurma>();
            services.AddScoped<IRepositorioParticipacaoTurma, RepositorioParticipacaoTurma>();
            services.AddScoped<IRepositorioParticipacaoTurmaAreaConhecimento, RepositorioParticipacaoTurmaAreaConhecimento>();
            services.AddScoped<IRepositorioParticipacaoUe, RepositorioParticipacaoUe>();
            services.AddScoped<IRepositorioParticipacaoUeAreaConhecimento, RepositorioParticipacaoUeAreaConhecimento>();
            services.AddScoped<IRepositorioParticipacaoDre, RepositorioParticipacaoDre>();
            services.AddScoped<IRepositorioParticipacaoDreAreaConhecimento, RepositorioParticipacaoDreAreaConhecimento>();
            services.AddScoped<IRepositorioParticipacaoSme, RepositorioParticipacaoSme>();
            services.AddScoped<IRepositorioParticipacaoSmeAreaConhecimento, RepositorioParticipacaoSmeAreaConhecimento>();
            services.AddScoped<IRepositorioResultadoCicloSme, RepositorioResultadoCicloSme>();
            services.AddScoped<IRepositorioResultadoCicloEscola, RepositorioResultadoCicloEscola>();
            services.AddScoped<IRepositorioResultadoCicloTurma, RepositorioResultadoCicloTurma>();
            services.AddScoped<IRepositorioResultadoCicloDre, RepositorioResultadoCicloDre>();

        }

        private static void RegistrarServicos(IServiceCollection services)
        {
            services.AddSingleton<IServicoLog, ServicoLog>();
            services.AddSingleton<IServicoMensageria, ServicoMensageria>();
        }

        private static void RegistrarCasosDeUso(IServiceCollection services)
        {
            services.AddScoped<ITratarProvasLegadoSyncUseCase, TratarProvasLegadoSyncUseCase>();
            services.AddScoped<ITratarProvaLegadoUseCase, TratarProvaLegadoUseCase>();
            services.AddScoped<ITratarProvaAnoLegadoUseCase, TratarProvaAnoLegadoUseCase>();
            services.AddScoped<ITratarAlternativaLegadoSyncUseCase, TratarAlternativaLegadoSyncUseCase>();
            services.AddScoped<ITratarAlternativaLegadoUseCase, TratarAlternativaLegadoLegadoUseCase>();
            services.AddScoped<ITratarQuestoesLegadoSyncUseCase, TratarQuestoesLegadoSyncUseCase>();
            services.AddScoped<ITratarProvaBIBSyncUseCase, TratarProvaBIBSyncUseCase>();
            services.AddScoped<ITratarProvaBIBUseCase, TratarProvaBIBUseCase>();
            services.AddScoped<IProvaWebPushTesteUseCase, ProvaWebPushTesteUseCase>();
            services.AddScoped<IAtualizaImagensQuestoesUseCase, AtualizaImagensQuestoesUseCase>();
            services.AddScoped<IAtualizaImagensAlternativasUseCase, AtualizaImagensAlternativasUseCase>();
            services.AddScoped<IIncluirRespostaAlunoUseCase, IncluirRespostaAlunoUseCase>();
            services.AddScoped<IIncluirPreferenciasAlunoUseCase, IncluirPreferenciasAlunoUseCase>();            
            services.AddScoped<ITratarFrequenciaAlunoProvaUseCase, TratarFrequenciaAlunoProvaUseCase>();
            services.AddScoped<ITratarFrequenciaAlunoProvaSyncUseCase, TratarFrequenciaAlunoProvaSyncUseCase>();

            services.AddScoped<ITratarProvaResultadoExtracaoUseCase, TratarProvaResultadoExtracaoUseCase>();
            services.AddScoped<ITratarProvaResultadoExtracaoFiltroUseCase, TratarProvaResultadoExtracaoFiltroUseCase>();
            services.AddScoped<IConsolidarProvaResultadoUseCase, ConsolidarProvaResultadoUseCase>();
            services.AddScoped<IConsolidarProvaRespostaPorFiltroUseCase, ConsolidarProvaRespostaPorFiltroUseCase>();
            services.AddScoped<IConsolidarProvaRespostaPorFiltroTurmaUseCase, ConsolidarProvaRespostaPorFiltroTurmaUseCase>();

            services.AddScoped<IIniciarProcessoFinalizarProvasAutomaticamenteUseCase, IniciarProcessoFinalizarProvasAutomaticamenteUseCase>();
            services.AddScoped<IFinalizarProvaAutomaticamenteUseCase, FinalizarProvaAutomaticamenteUseCase>();
            services.AddScoped<ITratarTipoProvaDeficienciaUseCase, TratarTipoProvaDeficienciaUseCase>();
            services.AddScoped<ITratarAlunoDeficienciaUseCase, TratarAlunoDeficienciaUseCase>();
            services.AddScoped<ITratarDownloadProvaAlunoUseCase, TratarDownloadProvaAlunoUseCase>();

            // sincronização institucional 
            services.AddScoped<IExecutarSincronizacaoInstitucionalDreSyncUseCase, ExecutarSincronizacaoInstitucionalDreSyncUseCase>();
            //services.AddScoped<IExecutarSincronizacaoInstitucionalDreTratarUseCase, ExecutarSincronizacaoInstitucionalDreTratarUseCase>();
            services.AddScoped<IExecutarSincronizacaoInstitucionalUeSyncUseCase, ExecutarSincronizacaoInstitucionalUeSyncUseCase>();
            services.AddScoped<IExecutarSincronizacaoInstitucionalUeTratarUseCase, ExecutarSincronizacaoInstitucionalUeTratarUseCase>();
            services.AddScoped<IExecutarSincronizacaoInstitucionalTurmaSyncUseCase, ExecutarSincronizacaoInstitucionalTurmaSyncUseCase>();
            services.AddScoped<IExecutarSincronizacaoInstitucionalAlunoSyncUseCase, ExecutarSincronizacaoInstitucionalAlunoSyncUseCase>();
            services.AddScoped<IExecutarSincronizacaoInstitucionalAlunoTratarUseCase, ExecutarSincronizacaoInstitucionalAlunoTratarUseCase>();
            services.AddScoped<IExecutarSincronizacaoTurmaAlunoHistoricoSyncUseCase, ExecutarSincronizacaoTurmaAlunoHistoricoSyncUseCase>();
            services.AddScoped<IExecutarSincronizacaoTurmaAlunoHistoricoTratarUseCase, ExecutarSincronizacaoTurmaAlunoHistoricoTratarUseCase>();

            //sincronizar adesão das provas 
            services.AddScoped<ITratarAdesaoProvaUseCase, TratarAdesaoProvaUseCase>();
            services.AddScoped<ITratarAdesaoProvaAlunoUseCase, TratarAdesaoProvaAlunoUseCase>();

            //sincronizar usuarios e grupos de usuarios coresso
            services.AddScoped<IExecutarSincronizacaoUsuarioCoreSsoUseCase, ExecutarSincronizacaoUsuarioCoreSsoUseCase>();
            services.AddScoped<ITratarUsuarioPorGrupoCoreSsoUseCase, TratarUsuarioPorGrupoCoreSsoUseCase>();
            services.AddScoped<ITratarUsuarioCoreSsoUseCase, TratarUsuarioCoreSsoUseCase>();
            services.AddScoped<ITratarUsuarioGrupoCoreSsoExcluirUseCase, TratarUsuarioGrupoCoreSsoExcluirUseCase>();

            //tratar abrangência usuário por grupo
            services.AddScoped<ITratarAbrangenciaUsuarioGrupoSerapUseCase, TratarAbrangenciaUsuarioGrupoSerapUseCase>();
            services.AddScoped<ITratarAbrangenciaGrupoExcluirUseCase, TratarAbrangenciaGrupoExcluirUseCase>();
            services.AddScoped<ITratarAbrangenciaUsuarioGrupoExcluirUseCase, TratarAbrangenciaUsuarioGrupoExcluirUseCase>();

            services.AddScoped<ITratarQuestaoCompletaSyncUseCase, TratarQuestaoCompletaSyncUseCase>();
            services.AddScoped<ITratarQuestaoCompletaUseCase, TratarQuestaoCompletaUseCase>();         
            services.AddScoped<IAjustarUeTurmasUseCase, AjustarUeTurmasUseCase>();
            services.AddScoped<ITratarAlunoProvaProficienciaAsyncUseCase, TratarAlunoProvaProficienciaAsyncUseCase>();
            services.AddScoped<ITratarAlunoProvaProficienciaUseCase, TratarAlunoProvaProficienciaUseCase>();
            services.AddScoped<ITratarCadernosProvaTaiUseCase, TratarCadernosProvaTaiUseCase>();
            services.AddScoped<ITratarCadernoAlunoProvaTaiUseCase, TratarCadernoAlunoProvaTaiUseCase>();
            // Persistencia Serap 

            services.AddScoped<IIncluirUsuarioSerapUseCase, IncluirAtualizarUsuarioSerapUseCase>();
            services.AddScoped<IAlterarUsuarioSerapUseCase, AlterarUsuarioSerapUseCase>();

            services.AddScoped<IAlterarProvaAlunoUseCase, AlterarProvaAlunoUseCase>();
            services.AddScoped<IIncluirUsuarioSerapUseCase, IncluirAtualizarUsuarioSerapUseCase>();

            services.AddScoped<IVersaoAppDispositivoAppUseCase, VersaoAppDispositivoAppUseCase>();

            services.AddScoped<IIncluirProvaAlunoUseCase, IncluirProvaAlunoUseCase>();

            services.AddScoped<IReabrirProvaAlunoUseCase, ReabrirProvaAlunoUseCase>();
            services.AddScoped<ITratarUsuarioDispositivoLoginUseCase, TratarUsuarioDispositivoLoginUseCase>();
            services.AddScoped<ITratarReaberturaProvaAlunoUseCase, TratarReaberturaProvaAlunoUseCase>();

            services.AddScoped<IProvaGrupoPermissaoUseCase, ProvaGrupoPermissaoUseCase>();
            services.AddScoped<ITratarOrdemQuestaoAlunoProvaTaiUseCase, TratarOrdemQuestaoAlunoProvaTaiUseCase>();
            services.AddScoped<ITratarProficienciaAlunoProvaTaiUseCase, TratarProficienciaAlunoProvaTaiUseCase>();

            services.AddScoped<ITratarStatusProcessoResultadoPspUseCase, TratarStatusProcessoResultadoPspUseCase>();
            services.AddScoped<IImportarProficienciaAlunoUseCase, ImportarProficienciaAlunoUseCase>();
            services.AddScoped<ITratarProficienciaAlunoUseCase, TratarProficienciaAlunoUseCase>();
            services.AddScoped<IImportarProficienciaSmeUseCase, ImportarProficienciaSmeUseCase>();
            services.AddScoped<ITratarProficienciaSmeUseCase, TratarProficienciaSmeUseCase>();
            services.AddScoped<IImportarProficienciaDreUseCase, ImportarProficienciaDreUseCase>();
            services.AddScoped<ITratarProficienciaDreUseCase, TratarProficienciaDreUseCase>();
            services.AddScoped<IImportarProficienciaEscolaUseCase, ImportarProficienciaEscolaUseCase>();
            services.AddScoped<ITratarProficienciaEscolaUseCase, TratarProficienciaEscolaUseCase>();
            services.AddScoped<IImportarProficienciaTurmaUseCase, ImportarProficienciaTurmaUseCase>();
            services.AddScoped<ITratarProficienciaTurmaUseCase, TratarProficienciaTurmaUseCase>();
            services.AddScoped<IImportarResultadoParticipacaoTurmaUseCase, ImportarResultadoParticipacaoTurmaUseCase>();
            services.AddScoped<ITratarResultadoParticipacaoTurmaUseCase, TratarResultadoParticipacaoTurmaUseCase>();
            services.AddScoped<IImportarParticipacaoTurmaAreaConhecimentoUseCase, ImportarParticipacaoTurmaAreaConhecimentoUseCase>();
            services.AddScoped<ITratarParticipacaoTurmaAreaConhecimentoUseCase, TratarParticipacaoTurmaAreaConhecimentoUseCase>();
            services.AddScoped<IImportarResultadoParticipacaoUeUseCase, ImportarResultadoParticipacaoUeUseCase>();
            services.AddScoped<ITratarResultadoParticipacaoUeUseCase, TratarResultadoParticipacaoUeUseCase>();
            services.AddScoped<IImportarParticipacaoUeAreaConhecimentoUseCase, ImportarParticipacaoUeAreaConhecimentoUseCase>();
            services.AddScoped<ITratarParticipacaoUeAreaConhecimentoUseCase, TratarParticipacaoUeAreaConhecimentoUseCase>();
            services.AddScoped<IImportarResultadoParticipacaoDreUseCase, ImportarResultadoParticipacaoDreUseCase>();
            services.AddScoped<ITratarResultadoParticipacaoDreUseCase, TratarResultadoParticipacaoDreUseCase>();
            services.AddScoped<IImportResultParticipDreAreaUseCaseUseCase, ImportResultParticipDreAreaUseCaseUseCase>();
            services.AddScoped<ITratarResultParticipDreAreaUseCase, TratarResultParticipDreAreaUseCase>();
            services.AddScoped<IImportarResultadoParticipacaoSmeUseCase, ImportarResultadoParticipacaoSmeUseCase>();
            services.AddScoped<ITratarResultadoParticipacaoSmeUseCase, TratarResultadoParticipacaoSmeUseCase>();
            services.AddScoped<IImportarResultadoParticipacaoSmeAreaConhecimentoUseCase, ImportarResultadoParticipacaoSmeAreaConhecimentoUseCase>();
            services.AddScoped<ITratarResultadoParticipacaoSmeAreaConhecimentoUseCase, TratarResultadoParticipacaoSmeAreaConhecimentoUseCase>();
            services.AddScoped<IImportarProficienciaCicloSmeUseCase, ImportarProficienciaCicloSmeUseCase>();
            services.AddScoped<ITratarProficienciaCicloSmeUseCase, TratarProficienciaCicloSmeUseCase>();
            services.AddScoped<IImportarProficienciaCicloEscolaUseCase, ImportarProficienciaCicloEscolaUseCase>();
            services.AddScoped<ITratarProficienciaCicloEscolaUseCase, TratarProficienciaCicloEscolaUseCase>();
            services.AddScoped<IImportarProficienciaCicloTurmaUseCase, ImportarProficienciaCicloTurmaUseCase>();
            services.AddScoped<ITratarProficienciaCicloTurmaUseCase, TratarProficienciaCicloTurmaUseCase>();            
            services.AddScoped<ITratarResultadoCicloDreUseCase, TratarResultadoCicloDreUseCase>();

        }
    }
}