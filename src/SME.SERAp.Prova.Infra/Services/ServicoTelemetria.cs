using Elastic.Apm;
using Microsoft.ApplicationInsights;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Infra
{
    public class ServicoTelemetria : IServicoTelemetria
    {
        private readonly TelemetriaOptions telemetriaOptions;
        public bool Apm => telemetriaOptions.Apm;

        public ServicoTelemetria(TelemetriaOptions telemetriaOptions)
        {
            this.telemetriaOptions = telemetriaOptions ?? throw new ArgumentNullException(nameof(telemetriaOptions));
        }

        public async Task<dynamic> RegistrarComRetornoAsync<T>(Func<Task<object>> acao, string acaoNome, string telemetriaNome, string telemetriaValor)
        {
            dynamic result = default;

            if (telemetriaOptions.Apm)
            {
                Stopwatch temporizadorApm = Stopwatch.StartNew();
                result = await acao() as dynamic;
                temporizadorApm.Stop();

                Agent.Tracer.CurrentTransaction.CaptureSpan(telemetriaNome, acaoNome, async (span) =>
                {
                    span.SetLabel(telemetriaNome, telemetriaValor);
                    span.Duration = temporizadorApm.Elapsed.TotalMilliseconds;
                });
            }
            else
            {
                result = await acao() as dynamic;
            }

            return result;
        }

        public dynamic RegistrarComRetorno<T>(Func<object> acao, string acaoNome, string telemetriaNome, string telemetriaValor)
        {
            dynamic result = default;

            if (telemetriaOptions.Apm)
            {
                Stopwatch temporizadorApm = Stopwatch.StartNew();
                result = acao();
                temporizadorApm.Stop();

                Agent.Tracer.CurrentTransaction.CaptureSpan(telemetriaNome, acaoNome, async (span) =>
                {
                    span.SetLabel(telemetriaNome, telemetriaValor);
                    span.Duration = temporizadorApm.Elapsed.TotalMilliseconds;
                });
            }
            else
            {
                result = acao();
            }

            return result;
        }

        public void Registrar(Action acao, string acaoNome, string telemetriaNome, string telemetriaValor)
        {
            if (telemetriaOptions.Apm)
            {
                Stopwatch temporizadorApm = Stopwatch.StartNew();
                acao();
                temporizadorApm.Stop();

                Agent.Tracer.CurrentTransaction.CaptureSpan(telemetriaNome, acaoNome, async (span) =>
                {
                    span.SetLabel(telemetriaNome, telemetriaValor);
                    span.Duration = temporizadorApm.Elapsed.TotalMilliseconds;
                });
            }
            else
            {
                acao();
            }
        }

        public async Task RegistrarAsync(Func<Task> acao, string acaoNome, string telemetriaNome, string telemetriaValor)
        {
            if (telemetriaOptions.Apm)
            {
                Stopwatch temporizadorApm = Stopwatch.StartNew();
                await acao();
                temporizadorApm.Stop();

                Agent.Tracer.CurrentTransaction.CaptureSpan(telemetriaNome, acaoNome, async (span) =>
                {
                    span.SetLabel(telemetriaNome, telemetriaValor);
                    span.Duration = temporizadorApm.Elapsed.TotalMilliseconds;
                });
            }
            else
            {
                await acao();
            }
        }
    }
}
