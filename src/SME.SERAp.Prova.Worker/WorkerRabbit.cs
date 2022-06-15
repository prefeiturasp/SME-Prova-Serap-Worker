using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sentry;
using SME.SERAp.Prova.Aplicacao.Interfaces;
using SME.SERAp.Prova.Infra;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using SME.SERAp.Prova.Infra.Exceptions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.Worker
{
    public class WorkerRabbit : BackgroundService
    {
        private readonly ILogger<WorkerRabbit> _logger;
        private readonly RabbitOptions rabbitOptions;
        private readonly SentryOptions sentryOptions;
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ConnectionFactory connectionFactory;
        private readonly Dictionary<string, ComandoRabbit> comandos;
        public WorkerRabbit(ILogger<WorkerRabbit> logger, RabbitOptions rabbitOptions, SentryOptions sentryOptions,
            IServiceScopeFactory serviceScopeFactory, ConnectionFactory connectionFactory)
        {
            _logger = logger;
            this.rabbitOptions = rabbitOptions ?? throw new ArgumentNullException(nameof(rabbitOptions));
            this.sentryOptions = sentryOptions ?? throw new ArgumentNullException(nameof(sentryOptions));
            this.serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
            this.connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
            comandos = new Dictionary<string, ComandoRabbit>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (SentrySdk.Init(sentryOptions))
            {
                using var conexaoRabbit = connectionFactory.CreateConnection();
                using IModel channel = conexaoRabbit.CreateModel();

                var props = channel.CreateBasicProperties();
                props.Persistent = true;

                channel.BasicQos(0, rabbitOptions.LimiteDeMensagensPorExecucao, false);

                channel.ExchangeDeclare(ExchangeRabbit.SerapEstudante, ExchangeType.Direct, true, false);
                channel.ExchangeDeclare(ExchangeRabbit.SerapEstudanteDeadLetter, ExchangeType.Direct, true, false);

                DeclararFilas(channel);

                RegistrarUseCases();

                await InicializaConsumer(channel, stoppingToken);
            }
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
                    SentrySdk.AddBreadcrumb($"Erro ao tratar mensagem {ea.DeliveryTag}", "erro", null, null, BreadcrumbLevel.Error);
                    SentrySdk.CaptureException(ex);
                    channel.BasicReject(ea.DeliveryTag, false);
                }
            };

            RegistrarConsumer(consumer, channel);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation(
                    $"Worker ativo em: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                await Task.Delay(10000, stoppingToken);
            }
        }
        private static void RegistrarConsumer(EventingBasicConsumer consumer, IModel channel)
        {
            foreach (var fila in typeof(RotasRabbit).ObterConstantesPublicas<string>())
                channel.BasicConsume(fila, false, consumer);
        }
        private static void DeclararFilas(IModel channel)
        {
            foreach (var fila in typeof(RotasRabbit).ObterConstantesPublicas<string>())
            {
                var args = new Dictionary<string, object>()
                    {
                        { "x-dead-letter-exchange", ExchangeRabbit.SerapEstudanteDeadLetter }
                    };

                channel.QueueDeclare(fila, true, false, false, args);
                channel.QueueBind(fila, ExchangeRabbit.SerapEstudante, fila, null);

                var filaDeadLetter = $"{fila}.deadletter";
                channel.QueueDeclare(filaDeadLetter, true, false, false, null);
                channel.QueueBind(filaDeadLetter, ExchangeRabbit.SerapEstudanteDeadLetter, fila, null);

                var filaDeadLetterFinal = $"{fila}.deadletter.final";
                channel.QueueDeclare(filaDeadLetterFinal, true, false, false, null);
                channel.QueueBind(filaDeadLetterFinal, ExchangeRabbit.SerapEstudanteDeadLetter, filaDeadLetterFinal, null);
            }
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

            comandos.Add(RotasRabbit.DownloadProvaAlunoTratar, new ComandoRabbit("tratar a situação do download da prova por aluno", typeof(ITratarDownloadProvaAlunoUseCase)));

            // Questao completa
            comandos.Add(RotasRabbit.QuestaoCompletaSync, new ComandoRabbit("Sincronização das questoes completas", typeof(ITratarQuestaoCompletaSyncUseCase)));
            comandos.Add(RotasRabbit.QuestaoCompletaTratar, new ComandoRabbit("Realiza a atualização dos dados completos da questão", typeof(ITratarQuestaoCompletaUseCase)));

            // proficiencia
            comandos.Add(RotasRabbit.AlunoProvaProficienciaAsync, new ComandoRabbit("Sincronização das proficiencia do aluno na prova", typeof(ITratarAlunoProvaProficienciaAsyncUseCase)));
            comandos.Add(RotasRabbit.AlunoProvaProficienciaTratar, new ComandoRabbit("Realiza a atuaização das proficiencia do aluno na prova", typeof(ITratarAlunoProvaProficienciaUseCase)));

            comandos.Add(RotasRabbit.ExtrairResultadosProva, new ComandoRabbit("Realizar a extração de uma prova", typeof(ITratarProvaResultadoExtracaoUseCase)));
            comandos.Add(RotasRabbit.ExtrairResultadosProvaFiltro, new ComandoRabbit("Realizar a extração de uma prova por filtro", typeof(ITratarProvaResultadoExtracaoFiltroUseCase)));
            comandos.Add(RotasRabbit.ConsolidarProvaResultado, new ComandoRabbit("Inicia consolidação dos dados da prova para exportação", typeof(IConsolidarProvaResultadoUseCase)));
            comandos.Add(RotasRabbit.ConsolidarProvaResultadoFiltro, new ComandoRabbit("Faz a consolidação dos dados da prova por filtro", typeof(IConsolidarProvaRespostaPorFiltroUseCase)));
            comandos.Add(RotasRabbit.ConsolidarProvaResultadoFiltroTurma, new ComandoRabbit("Faz a consolidação dos dados da prova por turma", typeof(IConsolidarProvaRespostaPorFiltroTurmaUseCase)));
            comandos.Add(RotasRabbit.TratarAdesaoProva, new ComandoRabbit("Faz a sincronização da adesão da prova do legado para o serap estudantes", typeof(ITratarAdesaoProvaUseCase)));
            comandos.Add(RotasRabbit.TratarTipoProvaDeficiencia, new ComandoRabbit("Faz a sincronização dos tipos de deficiência do tipo de prova", typeof(ITratarTipoProvaDeficienciaUseCase)));
            comandos.Add(RotasRabbit.TratarAlunoDeficiencia, new ComandoRabbit("Faz a sincronização dos tipos de deficiência do aluno", typeof(ITratarAlunoDeficienciaUseCase)));

            comandos.Add(RotasRabbit.ProvaWebPushTeste, new ComandoRabbit("Teste de webpush", typeof(IProvaWebPushTesteUseCase)));

            // Sincronização das UES e turmas
            comandos.Add(RotasRabbit.SincronizaEstruturaInstitucionalDreSync, new ComandoRabbit("Estrutura Institucional - Sync de Dre", typeof(IExecutarSincronizacaoInstitucionalDreSyncUseCase)));
            comandos.Add(RotasRabbit.SincronizaEstruturaInstitucionalUesSync, new ComandoRabbit("Estrutura Institucional - Sync de Ue", typeof(IExecutarSincronizacaoInstitucionalUeSyncUseCase)));
            comandos.Add(RotasRabbit.SincronizaEstruturaInstitucionalUeTratar, new ComandoRabbit("Estrutura Institucional - Tratar uma Ue", typeof(IExecutarSincronizacaoInstitucionalUeTratarUseCase)));
            comandos.Add(RotasRabbit.SincronizaEstruturaInstitucionalTurmasSync, new ComandoRabbit("Estrutura Institucional - Sincronizar Turmas/Alunos", typeof(IExecutarSincronizacaoInstitucionalTurmaSyncUseCase)));

            comandos.Add(RotasRabbit.SincronizaEstruturaInstitucionalAlunoSync, new ComandoRabbit("Estrutura Institucional - Sincronizar alunos", typeof(IExecutarSincronizacaoInstitucionalAlunoSyncUseCase)));
            comandos.Add(RotasRabbit.SincronizaEstruturaInstitucionalAlunoTratar, new ComandoRabbit("Estrutura Institucional - tratar alunos", typeof(IExecutarSincronizacaoInstitucionalAlunoTratarUseCase)));
            comandos.Add(RotasRabbit.SincronizaEstruturaInstitucionalTurmaAlunoHistoricoSync, new ComandoRabbit("Estrutura Institucional - Sincronizar turmas histórico alunos", typeof(IExecutarSincronizacaoTurmaAlunoHistoricoSyncUseCase)));
            comandos.Add(RotasRabbit.SincronizaEstruturaInstitucionalTurmaAlunoHistoricoTratar, new ComandoRabbit("Estrutura Institucional - Tratar turmas histórico alunos", typeof(IExecutarSincronizacaoTurmaAlunoHistoricoTratarUseCase)));

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

            comandos.Add(RotasRabbit.FilaDeadletterTratar, new ComandoRabbit("Tratamento de fila Deadletter", typeof(IRabbitDeadletterSerapTratarUseCase)));
            comandos.Add(RotasRabbit.FilaDeadletterSync, new ComandoRabbit("Sync de fila Deadletter", typeof(IRabbitDeadletterSerapSyncUseCase)));

            comandos.Add(RotasRabbit.SincronizaEstruturaInstitucionalAtualizarUeTurma, new ComandoRabbit("Atualizar escolas das turmas.", typeof(IAjustarUeTurmasUseCase)));
        }

        private static MethodInfo ObterMetodo(Type objType, string method)
        {
            var executar = objType.GetMethod(method);

            if (executar == null)
            {
                foreach (var itf in objType.GetInterfaces())
                {
                    executar = ObterMetodo(itf, method);
                    if (executar != null)
                        break;
                }
            }

            return executar;
        }

        private async Task TratarMensagem(BasicDeliverEventArgs ea, IModel channel)
        {
            var mensagem = Encoding.UTF8.GetString(ea.Body.Span);
            var rota = ea.RoutingKey;
            if (comandos.ContainsKey(rota))
            {
                var jsonSerializerOptions = new JsonSerializerOptions();
                jsonSerializerOptions.PropertyNameCaseInsensitive = true;

                var mensagemRabbit = mensagem.ConverterObjectStringPraObjeto<MensagemRabbit>();

                var comandoRabbit = comandos[rota];
                try
                {
                    using var scope = serviceScopeFactory.CreateScope();
                    var casoDeUso = scope.ServiceProvider.GetService(comandoRabbit.TipoCasoUso);

                    await ObterMetodo(comandoRabbit.TipoCasoUso, "Executar").InvokeAsync(casoDeUso, new object[] { mensagemRabbit });

                    channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (NegocioException nex)
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                    SentrySdk.AddBreadcrumb($"Erros: {nex.Message}", null, null, null, BreadcrumbLevel.Error);
                    SentrySdk.CaptureMessage($"Worker Serap: Rota -> {ea.RoutingKey}  Cod Correl -> {mensagemRabbit.CodigoCorrelacao.ToString().Substring(0, 3)}", SentryLevel.Error);
                    SentrySdk.CaptureException(nex);

                }
                catch (ValidacaoException vex)
                {
                    channel.BasicAck(ea.DeliveryTag, false);
                    SentrySdk.CaptureMessage($"Worker Serap: Rota -> {ea.RoutingKey}  Cod Correl -> {mensagemRabbit.CodigoCorrelacao.ToString().Substring(0, 3)}", SentryLevel.Error);
                    SentrySdk.AddBreadcrumb($"Erros: { JsonSerializer.Serialize(vex.Mensagens())}", null, null, null, BreadcrumbLevel.Error);
                    SentrySdk.CaptureException(vex);
                }
                catch (Exception ex)
                {
                    channel.BasicReject(ea.DeliveryTag, false);
                    SentrySdk.AddBreadcrumb($"Erros: {ex.Message}", null, null, null, BreadcrumbLevel.Error);
                    SentrySdk.CaptureException(ex);
                }
            }
            else
                channel.BasicReject(ea.DeliveryTag, false);
        }
    }
}
