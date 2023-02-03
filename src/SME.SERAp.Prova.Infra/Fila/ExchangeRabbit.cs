namespace SME.SERAp.Prova.Infra
{
    public class ExchangeRabbit
    {
        public static string SerapEstudante => "serap.estudante.workers";
        public static string Serap => "serap.workers";
        public static string SerapDeadLetter => "serap.workers.deadletter";
        public static string SerapEstudanteDeadLetter => "serap.estudante.workers.deadletter";
        public static string SerapEstudanteAcompanhamento => "serap.estudante.acomp.workers";
        public static string Logs => "EnterpriseApplicationLog";
        public static int SgpDeadLetterTtl => 10 * 60 * 1000; /*10 Min * 60 Seg * 1000 milisegundos = 10 minutos em milisegundos*/
    }
}
