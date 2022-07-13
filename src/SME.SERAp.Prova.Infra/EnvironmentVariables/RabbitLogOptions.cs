using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Infra.EnvironmentVariables
{
    public  class RabbitLogOptions
    {
        public const string Secao = "RabbitLog";
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
    }
}
