using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Dominio.Enums;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using SME.SERAp.Prova.Infra.Exceptions;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.Worker
{
    public class WorkerRabbit : BackgroundService
    {
        private readonly ILogger<WorkerRabbit> logger;
        private readonly RabbitOptions rabbitOptions;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ConnectionFactory connectionFactory;
        private readonly IServicoLog servicolog;
        private readonly IServicoTelemetria servicoTelemetria;
        private readonly IServicoMensageria servicoMensageria;
        private readonly Dictionary<string, ComandoRabbit> comandos;
        
        public WorkerRabbit(
            ILogger<WorkerRabbit> logger,
            RabbitOptions rabbitOptions,
            IServiceScopeFactory serviceScopeFactory,
            ConnectionFactory connectionFactory,
            IServicoLog servicolog,
            IServicoTelemetria servicoTelemetria,
            IServicoMensageria servicoMensageria)
        {
            this.logger = logger;
            this.rabbitOptions = rabbitOptions ?? throw new ArgumentNullException(nameof(rabbitOptions));
            this.serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            this.connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            this.servicolog = servicolog ?? throw new ArgumentNullException(nameof(servicolog));
            this.servicoTelemetria = servicoTelemetria ?? throw new ArgumentNullException(nameof(servicoTelemetria));
            this.servicoMensageria = servicoMensageria ?? throw new ArgumentNullException(nameof(servicoMensageria));
            comandos = new Dictionary<string, ComandoRabbit>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var conexaoRabbit = connectionFactory.CreateConnection();
            using var channel = conexaoRabbit.CreateModel();

            var props = channel.CreateBasicProperties();
            props.Persistent = true;

            channel.BasicQos(0, rabbitOptions.LimiteDeMensagensPorExecucao, false);

            channel.ExchangeDeclare(ExchangeRabbit.SerapEstudante, ExchangeType.Direct, true);
            channel.ExchangeDeclare(ExchangeRabbit.SerapEstudanteDeadLetter, ExchangeType.Direct, true);

            RegistrarUseCases();
            DeclararFilas(channel);

            await InicializaConsumer(channel, stoppingToken);

        }

        private async Task InicializaConsumer(IModel channel, CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (ch, ea) =>
            {
                try
                {
                    await TratarMensagem(ea, channel);
                }
                catch (Exception ex)
                {
                    servicolog.Registrar($"Erro ao tratar mensagem {ea.DeliveryTag}", ex);
                }
            };

            RegistrarConsumer(consumer, channel);

            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("Worker ativo em: {Now}", DateTime.Now);
                await Task.Delay(10000, stoppingToken);
            }
        }
        
        private static void RegistrarConsumer(EventingBasicConsumer consumer, IModel channel)
        {
            foreach (var fila in typeof(RotasRabbit).ObterConstantesPublicas<string>())
                channel.BasicConsume(fila, false, consumer);
        }
        
        private void DeclararFilas(IModel channel)
        {
            foreach (var fila in typeof(RotasRabbit).ObterConstantesPublicas<string>())
            {
                var filaDeadLetter = $"{fila}.deadletter";
                var filaDeadLetterFinal = $"{fila}.deadletter.final";
                
                if (rabbitOptions.ForcarRecriarFilas)
                {
                    channel.QueueDelete(fila, ifEmpty: true);
                    channel.QueueDelete(filaDeadLetter, ifEmpty: true);
                    channel.QueueDelete(filaDeadLetterFinal, ifEmpty: true);
                }                
                
                var args = ObterArgumentoDaFila(fila);
                channel.QueueDeclare(fila, true, false, false, args);
                channel.QueueBind(fila, ExchangeRabbit.SerapEstudante, fila, null);

                var argsDlq = ObterArgumentoDaFilaDeadLetter(fila);
                channel.QueueDeclare(filaDeadLetter, true, false, false, argsDlq);
                channel.QueueBind(filaDeadLetter, ExchangeRabbit.SerapEstudanteDeadLetter, fila, null);

                var argsFinal = new Dictionary<string, object> { { "x-queue-mode", "lazy" } };
                
                channel.QueueDeclare(
                    queue: filaDeadLetterFinal, 
                    durable: true, 
                    exclusive: false, 
                    autoDelete: false, 
                    arguments: argsFinal);
                
                channel.QueueBind(filaDeadLetterFinal, ExchangeRabbit.SerapEstudanteDeadLetter, filaDeadLetterFinal, null);
            }
        }
        
        private Dictionary<string, object> ObterArgumentoDaFila(string fila)
        {
            var args = new Dictionary<string, object>
                { { "x-dead-letter-exchange", ExchangeRabbit.SerapEstudanteDeadLetter } };

            if (comandos.ContainsKey(fila) && comandos[fila].ModeLazy)
                args.Add("x-queue-mode", "lazy");
            
            return args;
        }
        
        private Dictionary<string, object> ObterArgumentoDaFilaDeadLetter(string fila)
        {
            var argsDlq = new Dictionary<string, object>();
            var ttl = comandos.ContainsKey(fila) ? comandos[fila].Ttl : ExchangeRabbit.SerapDeadLetterTtl;

            argsDlq.Add("x-dead-letter-exchange", ExchangeRabbit.SerapEstudante);
            argsDlq.Add("x-message-ttl", ttl);
            argsDlq.Add("x-queue-mode", "lazy");

            return argsDlq;
        }
        
        private ulong GetRetryCount(IBasicProperties properties)
        {
            if (properties.Headers == null || !properties.Headers.ContainsKey("x-death"))
                return 0;
            
            var deathProperties = (List<object>)properties.Headers["x-death"];
            var lastRetry = (Dictionary<string, object>)deathProperties[0];
            var count = lastRetry["count"];
            
            return (ulong) Convert.ToInt64(count);
        }        

        private void RegistrarUseCases()
        {
            comandos.Add(RotasRabbit.ProvaSync, new ComandoRabbit("Sincronização da prova", typeof(ITratarProvasLegadoSyncUseCase)));
            comandos.Add(RotasRabbit.ProvaTratar, new ComandoRabbit("Tratar Prova", typeof(ITratarProvaLegadoUseCase)));
            comandos.Add(RotasRabbit.ProvaAnoTratar, new ComandoRabbit("Tratar Prova Ano", typeof(ITratarProvaAnoLegadoUseCase)));
            comandos.Add(RotasRabbit.QuestaoSync, new ComandoRabbit("Sincronização das questoes da prova", typeof(ITratarQuestoesLegadoSyncUseCase)));
            comandos.Add(RotasRabbit.AlternativaSync, new ComandoRabbit("Sincronização das alternativas da prova", typeof(ITratarAlternativaLegadoSyncUseCase)));
            comandos.Add(RotasRabbit.AlternativaTratar, new ComandoRabbit("Tratar as alternativas das provas", typeof(ITratarAlternativaLegadoUseCase)));

            comandos.Add(RotasRabbit.ProvaBIBSync, new ComandoRabbit("Sincronização das provas com BIB", typeof(ITratarProvaBIBSyncUseCase)));
            comandos.Add(RotasRabbit.ProvaBIBTratar, new ComandoRabbit("Tratar as provas com BIB", typeof(ITratarProvaBIBUseCase)));

            comandos.Add(RotasRabbit.QuestaoImagemIncorretaTratar, new ComandoRabbit("Atualizar questões com imagem incorreta", typeof(IAtualizaImagensQuestoesUseCase)));
            comandos.Add(RotasRabbit.AlternativaImagemIncorretaTratar, new ComandoRabbit("Atualizar alternativas com imagem incorreta", typeof(IAtualizaImagensAlternativasUseCase)));

            comandos.Add(RotasRabbit.IncluirRespostaAluno, new ComandoRabbit("Incluir as respostas do aluno", typeof(IIncluirRespostaAlunoUseCase)));
            comandos.Add(RotasRabbit.IncluirPreferenciasAluno, new ComandoRabbit("Incluir as preferências do sistema do aluno", typeof(IIncluirPreferenciasAlunoUseCase)));
            comandos.Add(RotasRabbit.AtualizarFrequenciaAlunoProvaTratar, new ComandoRabbit("Atualiza a prova do aluno com a frequência dele", typeof(ITratarFrequenciaAlunoProvaUseCase)));
            comandos.Add(RotasRabbit.AtualizarFrequenciaAlunoProvaSync, new ComandoRabbit("Obtem os alunos para serem atualizados", typeof(ITratarFrequenciaAlunoProvaSyncUseCase)));
            comandos.Add(RotasRabbit.ProvaGrupoPermissaoTratar, new ComandoRabbit("Trata as permissões de visualização das provas por grupo", typeof(IProvaGrupoPermissaoUseCase)));

            comandos.Add(RotasRabbit.DownloadProvaAlunoTratar, new ComandoRabbit("tratar a situação do download da prova por aluno", typeof(ITratarDownloadProvaAlunoUseCase)));

            // Questao completa
            comandos.Add(RotasRabbit.QuestaoCompletaSync, new ComandoRabbit("Sincronização das questoes completas", typeof(ITratarQuestaoCompletaSyncUseCase)));
            comandos.Add(RotasRabbit.QuestaoCompletaTratar, new ComandoRabbit("Realiza a atualização dos dados completos da questão", typeof(ITratarQuestaoCompletaUseCase)));

            // proficiencia
            comandos.Add(RotasRabbit.AlunoProvaProficienciaSync, new ComandoRabbit("Sincronização das proficiencia do aluno na prova", typeof(ITratarAlunoProvaProficienciaSyncUseCase)));
            comandos.Add(RotasRabbit.AlunoProvaProficienciaPorProvaSync, new ComandoRabbit("Sincronização das proficiencia do aluno por prova", typeof(ITratarAlunoProvaProficienciaPorProvaIdSyncUseCase)));            
            comandos.Add(RotasRabbit.AlunoProvaProficienciaTratar, new ComandoRabbit("Realiza a atuaização das proficiencia do aluno na prova", typeof(ITratarAlunoProvaProficienciaUseCase)));

            comandos.Add(RotasRabbit.ExtrairResultadosProva, new ComandoRabbit("Realizar a extração de uma prova", typeof(ITratarProvaResultadoExtracaoUseCase)));
            comandos.Add(RotasRabbit.ExtrairResultadosProvaFiltro, new ComandoRabbit("Realizar a extração de uma prova por filtro", typeof(ITratarProvaResultadoExtracaoFiltroUseCase)));
            comandos.Add(RotasRabbit.ConsolidarProvaResultado, new ComandoRabbit("Inicia consolidação dos dados da prova para exportação", typeof(IConsolidarProvaResultadoUseCase)));
            comandos.Add(RotasRabbit.ConsolidarProvaResultadoFiltro, new ComandoRabbit("Faz a consolidação dos dados da prova por filtro", typeof(IConsolidarProvaRespostaPorFiltroUseCase)));
            comandos.Add(RotasRabbit.ConsolidarProvaResultadoFiltroTurma, new ComandoRabbit("Faz a consolidação dos dados da prova por turma", typeof(IConsolidarProvaRespostaPorFiltroTurmaUseCase)));
            comandos.Add(RotasRabbit.TratarAdesaoProva, new ComandoRabbit("Faz a sincronização da adesão da prova do legado para o serap estudantes", typeof(ITratarAdesaoProvaUseCase)));
            comandos.Add(RotasRabbit.TratarAdesaoProvaAluno, new ComandoRabbit("Faz a sincronização da adesão do aluno na prova do legado para o serap estudantes", typeof(ITratarAdesaoProvaAlunoUseCase)));

            comandos.Add(RotasRabbit.TratarTipoProvaDeficiencia, new ComandoRabbit("Faz a sincronização dos tipos de deficiência do tipo de prova", typeof(ITratarTipoProvaDeficienciaUseCase)));
            comandos.Add(RotasRabbit.TratarAlunoDeficiencia, new ComandoRabbit("Faz a sincronização dos tipos de deficiência do aluno", typeof(ITratarAlunoDeficienciaUseCase)));

            comandos.Add(RotasRabbit.ProvaWebPushTeste, new ComandoRabbit("Teste de webpush", typeof(IProvaWebPushTesteUseCase)));

            // Sincronização das UES e turmas
            comandos.Add(RotasRabbit.SincronizaEstruturaInstitucionalDreSync, new ComandoRabbit("Estrutura Institucional - Sincronizar Dres", typeof(IExecutarSincronizacaoInstitucionalDreSyncUseCase)));
            comandos.Add(RotasRabbit.SincronizaEstruturaInstitucionalDreTratar, new ComandoRabbit("Estrutura Institucional - Tratar uma Dre", typeof(IExecutarSincronizacaoInstitucionalDreTratarUseCase)));
            comandos.Add(RotasRabbit.SincronizaEstruturaInstitucionalUesSync, new ComandoRabbit("Estrutura Institucional - Sincronizar Ues", typeof(IExecutarSincronizacaoInstitucionalUeSyncUseCase)));
            comandos.Add(RotasRabbit.SincronizaEstruturaInstitucionalUeTratar, new ComandoRabbit("Estrutura Institucional - Tratar uma Ue", typeof(IExecutarSincronizacaoInstitucionalUeTratarUseCase)));
            comandos.Add(RotasRabbit.SincronizaEstruturaInstitucionalTurmasSync, new ComandoRabbit("Estrutura Institucional - Sincronizar Turmas", typeof(IExecutarSincronizacaoInstitucionalTurmaSyncUseCase)));
            comandos.Add(RotasRabbit.SincronizaEstruturaInstitucionalTurmaTratar, new ComandoRabbit("Estrutura Institucional - Tratar turmas", typeof(IExecutarSincronizacaoInstitucionalTurmaTratarUseCase)));
            comandos.Add(RotasRabbit.SincronizaEstruturaInstitucionalAlunoSync, new ComandoRabbit("Estrutura Institucional - Sincronizar alunos", typeof(IExecutarSincronizacaoInstitucionalAlunoSyncUseCase)));
            comandos.Add(RotasRabbit.SincronizaEstruturaInstitucionalAlunoTratar, new ComandoRabbit("Estrutura Institucional - Tratar alunos", typeof(IExecutarSincronizacaoInstitucionalAlunoTratarUseCase)));
            comandos.Add(RotasRabbit.SincronizaEstruturaInstitucionalTurmaAlunoHistoricoSync, new ComandoRabbit("Estrutura Institucional - Sincronizar turmas histórico alunos", typeof(IExecutarSincronizacaoTurmaAlunoHistoricoSyncUseCase)));
            comandos.Add(RotasRabbit.SincronizaEstruturaInstitucionalTurmaAlunoHistoricoTratar, new ComandoRabbit("Estrutura Institucional - Tratar turmas histórico alunos", typeof(IExecutarSincronizacaoTurmaAlunoHistoricoTratarUseCase)));
            comandos.Add(RotasRabbit.SincronizaEstruturaInstitucionalAtualizarUeTurma, new ComandoRabbit("Atualizar escolas das turmas.", typeof(IAjustarUeTurmasUseCase)));            

            //Finalizar provas automaticamente
            comandos.Add(RotasRabbit.IniciarProcessoFinalizarProvasAutomaticamente, new ComandoRabbit("Finalizar provas automaticamente - Iniciar novo processo", typeof(IIniciarProcessoFinalizarProvasAutomaticamenteUseCase)));
            comandos.Add(RotasRabbit.FinalizarProvaAutomaticamente, new ComandoRabbit("Finalizar provas automaticamente - Finalizar provas dos alunos", typeof(IFinalizarProvaAutomaticamenteUseCase)));

            comandos.Add(RotasRabbit.UsuarioCoreSsoSync, new ComandoRabbit("Inicia a sincronização de usuarios do CoreSSO por grupos", typeof(IExecutarSincronizacaoUsuarioCoreSsoUseCase)));
            comandos.Add(RotasRabbit.UsuarioPorGrupoCoreSsoTratar, new ComandoRabbit("Trata usuarios do CoreSSO por grupo para sincronizar", typeof(ITratarUsuarioPorGrupoCoreSsoUseCase)));
            comandos.Add(RotasRabbit.UsuarioCoreSsoTratar, new ComandoRabbit("Sincroniza usuarios do CoreSSO para a base do serap estudantes", typeof(ITratarUsuarioCoreSsoUseCase)));
            comandos.Add(RotasRabbit.UsuarioGrupoCoreSsoExcluirTratar, new ComandoRabbit("Trata usuarios do CoreSSO por grupo para sincronizar", typeof(ITratarUsuarioGrupoCoreSsoExcluirUseCase)));

            comandos.Add(RotasRabbit.UsuarioGrupoAbrangenciaTratar, new ComandoRabbit("Trata abrangência dos usuarios do CoreSSO por grupo", typeof(ITratarAbrangenciaUsuarioGrupoSerapUseCase)));
            comandos.Add(RotasRabbit.GrupoAbrangenciaExcluir, new ComandoRabbit("Busca abrangências por grupo para fila excluir", typeof(ITratarAbrangenciaGrupoExcluirUseCase)));
            comandos.Add(RotasRabbit.UsuarioGrupoAbrangenciaExcluirTratar, new ComandoRabbit("", typeof(ITratarAbrangenciaUsuarioGrupoExcluirUseCase)));

            //Prova TAI
            comandos.Add(RotasRabbit.TratarCadernosProvaTai, new ComandoRabbit("Tratamento cadernos amostra TAI", typeof(ITratarCadernosProvaTaiUseCase)));
            comandos.Add(RotasRabbit.TratarCadernoAlunoProvaTai, new ComandoRabbit("Tratamento cadernos alunos prova TAI", typeof(ITratarCadernoAlunoProvaTaiUseCase)));
            comandos.Add(RotasRabbit.TratarOrdemQuestaoAlunoProvaTai, new ComandoRabbit("Tratamento da ordem da questão da prova tai do aluno", typeof(ITratarOrdemQuestaoAlunoProvaTaiUseCase)));
            comandos.Add(RotasRabbit.TratarProficienciaAlunoProvaTai, new ComandoRabbit("Tratamento da proficiencia da prova tai do aluno", typeof(ITratarProficienciaAlunoProvaTaiUseCase)));            

            // Persistencia Serap Estudantes
            comandos.Add(RotasRabbit.IncluirUsuario, new ComandoRabbit("Incluir Usuario Persistencia Serap", typeof(IIncluirUsuarioSerapUseCase)));
            comandos.Add(RotasRabbit.AlterarUsuario, new ComandoRabbit("Alterar Usuario Persistencia Serap", typeof(IAlterarUsuarioSerapUseCase)));
            comandos.Add(RotasRabbit.IncluirProvaAluno, new ComandoRabbit("Incluir Prova Aluno Serap Estudantes", typeof(IIncluirProvaAlunoUseCase)));
            comandos.Add(RotasRabbit.IncluirVersaoDispositivoApp, new ComandoRabbit("Incluir Versao Dispositivo App", typeof(IVersaoAppDispositivoAppUseCase)));

            comandos.Add(RotasRabbit.ReabrirProvaAluno, new ComandoRabbit("Reabrir Prova Aluno Serap estudantes", typeof(IReabrirProvaAlunoUseCase)));
            comandos.Add(RotasRabbit.TratarUsuarioDispositivoLogin, new ComandoRabbit("Salvar dispositivo no login do usuário", typeof(ITratarUsuarioDispositivoLoginUseCase)));
            comandos.Add(RotasRabbit.TratarReaberturaProvaAluno, new ComandoRabbit("Tramento de reabertura de prova de aluno serap estudantes", typeof(ITratarReaberturaProvaAlunoUseCase)));

            comandos.Add(RotasRabbit.TratarStatusProcessoResultado, new ComandoRabbit("Tratar status do processo", typeof(ITratarStatusProcessoResultadoPspUseCase)));
            comandos.Add(RotasRabbit.ImportarResultadoAlunoPsp, new ComandoRabbit("Importa arquivo csv proeficiencia aluno", typeof(IImportarProficienciaAlunoUseCase)));
            comandos.Add(RotasRabbit.TratarResultadoAlunoPsp, new ComandoRabbit("Tratar resgistros arquivo csv proeficiencia aluno", typeof(ITratarProficienciaAlunoUseCase)));
            comandos.Add(RotasRabbit.ImportarResultadoSmePsp, new ComandoRabbit("Importa dados arquivo csv proficiencia sme", typeof(IImportarProficienciaSmeUseCase)));
            comandos.Add(RotasRabbit.TratarResultadoSmePsp, new ComandoRabbit("Tratar resgistros arquivo csv proficiencia sme", typeof(ITratarProficienciaSmeUseCase)));
            comandos.Add(RotasRabbit.ImportarResultadoDrePsp, new ComandoRabbit("Importa dados arquivo csv proficiencia dre", typeof(IImportarProficienciaDreUseCase)));
            comandos.Add(RotasRabbit.TratarResultadoDrePsp, new ComandoRabbit("Tratar resgistros arquivo csv proficiencia dre", typeof(ITratarProficienciaDreUseCase)));
            comandos.Add(RotasRabbit.ImportarResultadoEscolaPsp, new ComandoRabbit("Importa dados arquivo csv proficiencia escola", typeof(IImportarProficienciaEscolaUseCase)));
            comandos.Add(RotasRabbit.TratarResultadoEscolaPsp, new ComandoRabbit("Tratar resgistros arquivo csv proficiencia escola", typeof(ITratarProficienciaEscolaUseCase)));
            comandos.Add(RotasRabbit.ImportarResultadoTurmaPsp, new ComandoRabbit("Importa dados arquivo csv proficiencia turma", typeof(IImportarProficienciaTurmaUseCase)));
            comandos.Add(RotasRabbit.TratarResultadoTurmaPsp, new ComandoRabbit("Tratar resgistros arquivo csv proficiencia turma", typeof(ITratarProficienciaTurmaUseCase)));
            comandos.Add(RotasRabbit.ImportarResultadoParticipacaoTurma, new ComandoRabbit("Importa dados arquivo csv participação turma", typeof(IImportarResultadoParticipacaoTurmaUseCase)));
            comandos.Add(RotasRabbit.TratarResultadoParticipacaoTurma, new ComandoRabbit("Tratar registros arquivo csv participação turma", typeof(ITratarResultadoParticipacaoTurmaUseCase)));
            comandos.Add(RotasRabbit.ImportarParticipacaoTurmaAreaConhecimento, new ComandoRabbit("Importa dados arquivo csv participação turma e area conhecimento", typeof(IImportarParticipacaoTurmaAreaConhecimentoUseCase)));
            comandos.Add(RotasRabbit.TratarParticipacaoTurmaAreaConhecimento, new ComandoRabbit("Tratar registros arquivo csv participação turma e area conhecimento", typeof(ITratarParticipacaoTurmaAreaConhecimentoUseCase)));
            comandos.Add(RotasRabbit.ImportarResultadoParticipacaoUe, new ComandoRabbit("Importa dados arquivo csv participação Ue", typeof(IImportarResultadoParticipacaoUeUseCase)));
            comandos.Add(RotasRabbit.TratarResultadoParticipacaoUe, new ComandoRabbit("Tratar registros arquivo csv participação Ue", typeof(ITratarResultadoParticipacaoUeUseCase)));
            comandos.Add(RotasRabbit.ImportarParticipacaoUeAreaConhecimento, new ComandoRabbit("Importa dados arquivo csv participação Ue e area conhecimento", typeof(IImportarParticipacaoUeAreaConhecimentoUseCase)));
            comandos.Add(RotasRabbit.TratarParticipacaoUeAreaConhecimento, new ComandoRabbit("Tratar registros arquivo csv participação Ue e area conhecimento", typeof(ITratarParticipacaoUeAreaConhecimentoUseCase)));
            comandos.Add(RotasRabbit.ImportarResultadoParticipacaoDre, new ComandoRabbit("Importa dados arquivo csv participação Dre", typeof(IImportarResultadoParticipacaoDreUseCase)));
            comandos.Add(RotasRabbit.TratarResultadoParticipacaoDre, new ComandoRabbit("Tratar registros arquivo csv participação Dre", typeof(ITratarResultadoParticipacaoDreUseCase)));
            comandos.Add(RotasRabbit.ImportarResultadoParticipacaoDreAreaConhecimento, new ComandoRabbit("Importa dados arquivo csv participação Dre AreaConhecimento", typeof(IImportResultParticipDreAreaUseCaseUseCase)));
            comandos.Add(RotasRabbit.TratarResultadoParticipacaoDreAreaConhecimento, new ComandoRabbit("Tratar registros arquivo csv participação Dre AreaConhecimento", typeof(ITratarResultParticipDreAreaUseCase)));
            comandos.Add(RotasRabbit.ImportarResultadoParticipacaoSme, new ComandoRabbit("Importa dados arquivo csv participação Sme", typeof(IImportarResultadoParticipacaoSmeUseCase)));
            comandos.Add(RotasRabbit.TratarResultadoParticipacaoSme, new ComandoRabbit("Tratar registros arquivo csv participação Sme", typeof(ITratarResultadoParticipacaoSmeUseCase)));
            comandos.Add(RotasRabbit.ImportarResultadoParticipacaoSmeAreaConhecimento, new ComandoRabbit("Importa dados arquivo csv participação Sme AreaConhecimento", typeof(IImportarResultadoParticipacaoSmeAreaConhecimentoUseCase)));
            comandos.Add(RotasRabbit.TratarResultadoParticipacaoSmeAreaConhecimento, new ComandoRabbit("Tratar registros arquivo csv participação Sme AreaConhecimento", typeof(ITratarResultadoParticipacaoSmeAreaConhecimentoUseCase)));

        }

        private static MethodInfo ObterMetodo(Type objType, string method)
        {
            var executar = objType.GetMethod(method);

            if (executar != null) 
                return executar;
            
            foreach (var itf in objType.GetInterfaces())
            {
                executar = ObterMetodo(itf, method);

                if (executar != null)
                    break;
            }

            return executar;
        }

        private async Task TratarMensagem(BasicDeliverEventArgs ea, IModel channel)
        {
            var mensagem = Encoding.UTF8.GetString(ea.Body.Span);
            var rota = ea.RoutingKey;

            if (comandos.ContainsKey(rota))
            {
                logger.LogInformation("Worker rota: {Rota}", rota);
                var transacao = servicoTelemetria.IniciarTransacao(rota);

                var mensagemRabbit = mensagem.ConverterObjectStringPraObjeto<MensagemRabbit>();
                var comandoRabbit = comandos[rota];

                try
                {
                    using var scope = serviceScopeFactory.CreateScope();
                    var casoDeUso = scope.ServiceProvider.GetService(comandoRabbit.TipoCasoUso);

                    await ObterMetodo(comandoRabbit.TipoCasoUso, "Executar").InvokeAsync(casoDeUso, mensagemRabbit);

                    channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (NegocioException nex)
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                    servicolog.Registrar(LogNivel.Negocio, $"Rota-- {ea.RoutingKey} -- Erros: {nex.Message}", $"Mensagem Rabbit: {mensagemRabbit.Mensagem} --", nex.StackTrace);
                    servicoTelemetria.RegistrarExcecao(transacao, nex);
                }
                catch (ValidacaoException vex)
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                    servicolog.Registrar(LogNivel.Negocio, $"Rota-- {ea.RoutingKey} -- Erros: {vex.Message}", $"Mensagem Rabbit: {mensagemRabbit.Mensagem} --", vex.StackTrace);
                    servicoTelemetria.RegistrarExcecao(transacao, vex);
                }
                catch (Exception ex)
                {
                    servicoTelemetria.RegistrarExcecao(transacao, ex);
                    
                    var rejeicoes = GetRetryCount(ea.BasicProperties);

                    if (++rejeicoes >= comandoRabbit.QuantidadeReprocessamentoDeadLetter)
                    {
                        channel.BasicAck(ea.DeliveryTag, false);
                        
                        var filaFinal = $"{ea.RoutingKey}.deadletter.final";

                        await servicoMensageria.Publicar(mensagemRabbit, filaFinal,
                            ExchangeRabbit.SerapEstudanteDeadLetter,
                            "PublicarDeadLetter");
                    } else
                        channel.BasicReject(ea.DeliveryTag, false);
                    
                    servicolog.Registrar(LogNivel.Critico, $"Rota-- {ea.RoutingKey} -- Erros: {ex.Message}", $"Mensagem Rabbit: {mensagemRabbit.Mensagem} --", ex.StackTrace);
                    
                }
                finally
                {
                    servicoTelemetria.FinalizarTransacao(transacao);
                }
            }
            else
                channel.BasicReject(ea.DeliveryTag, false);
        }
    }
}
