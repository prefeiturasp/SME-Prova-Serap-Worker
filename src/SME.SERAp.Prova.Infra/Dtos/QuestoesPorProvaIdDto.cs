namespace SME.SERAp.Prova.Infra
{
    public class QuestoesPorProvaIdDto
    {
        public long QuestaoId  { get; set; }
        public int Orderm  { get; set; }
        public long ProvaLegadoId  { get; set; }
        public string Questao  { get; set; }
        public string Enunciado  { get; set; }

        public QuestoesPorProvaIdDto(long questaoId, int orderm, long provaLegadoId, string questao, string enunciado)
        {
            QuestaoId = questaoId;
            Orderm = orderm;
            ProvaLegadoId = provaLegadoId;
            Questao = questao;
            Enunciado = enunciado;
        }

        public QuestoesPorProvaIdDto()
        {
        }
    }
}
