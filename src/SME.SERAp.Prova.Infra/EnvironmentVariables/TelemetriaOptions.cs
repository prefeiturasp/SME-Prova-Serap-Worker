﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Infra.EnvironmentVariables
{
    public class TelemetriaOptions
    {
        public const string Secao = "Telemetria";
        public bool ApplicationInsights { get; set; }
        public bool Apm { get; set; }
    }
}

