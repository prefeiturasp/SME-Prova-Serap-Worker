using Elastic.Apm;
using Elastic.Apm.Api;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Infra
{
    public class ServicoTelemetria : IServicoTelemetria
    {
        private readonly TelemetryClient insightsClient;
        private readonly TelemetriaOptions telemetriaOptions;

        public ServicoTelemetria(TelemetryClient insightsClient, TelemetriaOptions telemetriaOptions)
        {
            this.insightsClient = insightsClient ?? throw new ArgumentNullException(nameof(insightsClient));
            this.telemetriaOptions = telemetriaOptions ?? throw new ArgumentNullException(nameof(telemetriaOptions));
        }

        public ServicoTelemetriaTransacao IniciarTransacao(string rota)
        {
            var transacao = new ServicoTelemetriaTransacao(rota);

            if (telemetriaOptions.Apm)
                transacao.TransacaoApm = Agent.Tracer.StartTransaction(rota, "WorkerRabbitSerap");

            if (telemetriaOptions.ApplicationInsights)
            {
                transacao.InicioOperacao = DateTime.UtcNow;
                transacao.Temporizador = Stopwatch.StartNew();
            }

            return transacao;
        }

        public void FinalizarTransacao(ServicoTelemetriaTransacao servicoTelemetriaTransacao)
        {
            if (telemetriaOptions.Apm)
                servicoTelemetriaTransacao.TransacaoApm?.End();

            if (telemetriaOptions.ApplicationInsights)
            {
                if (servicoTelemetriaTransacao.Sucesso)
                {
                    servicoTelemetriaTransacao.Temporizador.Stop();
                    insightsClient?.TrackRequest(
                        servicoTelemetriaTransacao.Nome,
                        servicoTelemetriaTransacao.InicioOperacao,
                        servicoTelemetriaTransacao.Temporizador.Elapsed,
                        servicoTelemetriaTransacao.Sucesso ? "200" : "500",
                        servicoTelemetriaTransacao.Sucesso
                        );
                }
            }
        }

        public void RegistrarExcecao(ServicoTelemetriaTransacao servicoTelemetriaTransacao, Exception ex)
        {
            if (telemetriaOptions.Apm)
                servicoTelemetriaTransacao.TransacaoApm?.CaptureException(ex);

            if (telemetriaOptions.ApplicationInsights)
            {
                servicoTelemetriaTransacao.Sucesso = false;
                servicoTelemetriaTransacao.Temporizador.Stop();
                insightsClient?.TrackRequest(
                    servicoTelemetriaTransacao.Nome,
                    servicoTelemetriaTransacao.InicioOperacao,
                    servicoTelemetriaTransacao.Temporizador.Elapsed,
                    "500",
                    servicoTelemetriaTransacao.Sucesso
                    );

                insightsClient?.TrackDependency(
                    servicoTelemetriaTransacao.Nome,
                    ex.Message,
                    ex.StackTrace,
                    servicoTelemetriaTransacao.InicioOperacao,
                    servicoTelemetriaTransacao.Temporizador.Elapsed,
                    servicoTelemetriaTransacao.Sucesso);
                insightsClient.TrackException(ex);
            }
        }

        public async Task<dynamic> RegistrarComRetornoAsync<T>(Func<Task<object>> acao, string acaoNome, string telemetriaNome, string telemetriaValor)
        {
            dynamic result = default;
            DateTime inicioOperacao = default;
            Stopwatch temporizador = default;

            if (telemetriaOptions.ApplicationInsights)
            {
                inicioOperacao = DateTime.UtcNow;
                temporizador = Stopwatch.StartNew();
            }

            if (telemetriaOptions.Apm)
            {
                Stopwatch temporizadorApm = Stopwatch.StartNew();
                result = await acao() as dynamic;
                temporizadorApm.Stop();

                Agent.Tracer.CurrentTransaction.CaptureSpan(telemetriaNome, acaoNome, (span) =>
                {
                    span.SetLabel(telemetriaNome, telemetriaValor);
                    span.Duration = temporizadorApm.Elapsed.TotalMilliseconds;
                });
            }
            else
            {
                result = await acao() as dynamic;
            }

            if (telemetriaOptions.ApplicationInsights)
            {
                temporizador.Stop();
                insightsClient?.TrackDependency(acaoNome, telemetriaNome, telemetriaValor, inicioOperacao, temporizador.Elapsed, true);
            }

            return result;
        }

        public dynamic RegistrarComRetorno<T>(Func<object> acao, string acaoNome, string telemetriaNome, string telemetriaValor)
        {
            dynamic result = default;
            DateTime inicioOperacao = default;
            Stopwatch temporizador = default;

            if (telemetriaOptions.ApplicationInsights)
            {
                inicioOperacao = DateTime.UtcNow;
                temporizador = Stopwatch.StartNew();
            }

            if (telemetriaOptions.Apm)
            {
                Stopwatch temporizadorApm = Stopwatch.StartNew();
                result = acao();
                temporizadorApm.Stop();

                Agent.Tracer.CurrentTransaction.CaptureSpan(telemetriaNome, acaoNome, (span) =>
                {
                    span.SetLabel(telemetriaNome, telemetriaValor);
                    span.Duration = temporizadorApm.Elapsed.TotalMilliseconds;
                });
            }
            else
            {
                result = acao();
            }

            if (telemetriaOptions.ApplicationInsights)
            {
                temporizador.Stop();
                insightsClient?.TrackDependency(acaoNome, telemetriaNome, telemetriaValor, inicioOperacao, temporizador.Elapsed, true);
            }

            return result;
        }

        public void Registrar(Action acao, string acaoNome, string telemetriaNome, string telemetriaValor)
        {
            DateTime inicioOperacao = default;
            Stopwatch temporizador = default;

            if (telemetriaOptions.ApplicationInsights)
            {
                inicioOperacao = DateTime.UtcNow;
                temporizador = Stopwatch.StartNew();
            }

            if (telemetriaOptions.Apm)
            {
                Stopwatch temporizadorApm = Stopwatch.StartNew();
                acao();
                temporizadorApm.Stop();

                Agent.Tracer.CurrentTransaction.CaptureSpan(telemetriaNome, acaoNome, (span) =>
                {
                    span.SetLabel(telemetriaNome, telemetriaValor);
                    span.Duration = temporizadorApm.Elapsed.TotalMilliseconds;
                });
            }
            else
            {
                acao();
            }

            if (telemetriaOptions.ApplicationInsights)
            {
                temporizador.Stop();
                insightsClient?.TrackDependency(acaoNome, telemetriaNome, telemetriaValor, inicioOperacao, temporizador.Elapsed, true);
            }
        }

        public async Task RegistrarAsync(Func<Task> acao, string acaoNome, string telemetriaNome, string telemetriaValor)
        {
            DateTime inicioOperacao = default;
            Stopwatch temporizador = default;

            if (telemetriaOptions.ApplicationInsights)
            {
                inicioOperacao = DateTime.UtcNow;
                temporizador = Stopwatch.StartNew();
            }

            if (telemetriaOptions.Apm)
            {
                Stopwatch temporizadorApm = Stopwatch.StartNew();
                await acao();
                temporizadorApm.Stop();

                Agent.Tracer.CurrentTransaction.CaptureSpan(telemetriaNome, acaoNome, (span) =>
                {
                    span.SetLabel(telemetriaNome, telemetriaValor);
                    span.Duration = temporizadorApm.Elapsed.TotalMilliseconds;
                });
            }
            else
            {
                await acao();
            }

            if (telemetriaOptions.ApplicationInsights)
            {
                temporizador.Stop();
                insightsClient?.TrackDependency(acaoNome, telemetriaNome, telemetriaValor, inicioOperacao, temporizador.Elapsed, true);
            }
        }

        public class ServicoTelemetriaTransacao
        {
            public ServicoTelemetriaTransacao(string nome)
            {
                Nome = nome;
                Sucesso = true;
            }

            public string Nome { get; set; }
            public ITransaction TransacaoApm { get; set; }
            public DateTime InicioOperacao { get; set; }
            public Stopwatch Temporizador { get; set; }
            public bool Sucesso { get; set; }
        }
    }
}
