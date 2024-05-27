﻿namespace SME.SERAp.Prova.Infra.EnvironmentVariables
{
    public class ElasticOptions
    {
        public static string Secao => "ElasticSearch";
        public string Urls { get; set; }
        public string DefaultIndex { get; set; }
        public string PrefixIndex { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}