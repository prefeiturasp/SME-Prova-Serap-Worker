namespace SME.SERAp.Prova.Infra
{
    public class QuestoesPorProvaIdDto
    {
        public long QuestaoId  { get; set; }
        public int Ordem  { get; set; }
        public long ProvaLegadoId  { get; set; }
        public string Questao  { get; set; }
        public string Enunciado  { get; set; }
        public string Caderno { get; set; }
        public int TipoItem { get; set; }
        public int QuantidadeAlternativas { get; set; }

        public QuestoesPorProvaIdDto(long questaoId, int ordem, long provaLegadoId, string questao, string enunciado, int tipoItem, int quantidadeAlternativas, string caderno)
        {
            QuestaoId = questaoId;
            Ordem = ordem;
            ProvaLegadoId = provaLegadoId;
            Questao = questao;
            Enunciado = enunciado;
            TipoItem = tipoItem;
            QuantidadeAlternativas = quantidadeAlternativas;
            Caderno = caderno;
        }

        public QuestoesPorProvaIdDto()
        {
        }
    }
}
