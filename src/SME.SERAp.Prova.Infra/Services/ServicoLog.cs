using Sentry;
using SME.SERAp.Prova.Infra.EnvironmentVariables;
using SME.SERAp.Prova.Infra.Interfaces;
using System;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Infra.Services
{
    public class ServicoLog : IServicoLog
    {
        private readonly LogOptions logOptions;

        public ServicoLog(LogOptions logOptions)
        {
            this.logOptions = logOptions ?? throw new ArgumentNullException(nameof(logOptions));
        }

        public void Registrar(string mensagem)
        {
            using (SentrySdk.Init(logOptions.SentryDSN))
            {
                SentrySdk.CaptureMessage(mensagem);
            }
        }

        public void Registrar(Exception ex)
        {
            using (SentrySdk.Init(logOptions.SentryDSN))
            {
                SentrySdk.CaptureException(ex);
            }
        }        
    }
}
