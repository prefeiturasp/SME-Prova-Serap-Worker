namespace SME.SERAp.Prova.Infra
{
    public class QuestoesPorProvaIdDto
    {
        public long QuestaoId  { get; set; }
        public int Ordem  { get; set; }
        public long ProvaLegadoId  { get; set; }
        public string Questao  { get; set; }
        public string Enunciado  { get; set; }
        public int TipoItem { get; set; }
        public string TipoItemDescricao { get; set; }

        public QuestoesPorProvaIdDto(long questaoId, int ordem, long provaLegadoId, string questao, string enunciado, int tipoItem, string tipoItemDescricao)
        {
            QuestaoId = questaoId;
            Ordem = ordem;
            ProvaLegadoId = provaLegadoId;
            Questao = questao;
            Enunciado = enunciado;
            TipoItem = tipoItem;
            TipoItemDescricao = tipoItemDescricao;
        }

        public QuestoesPorProvaIdDto()
        {
        }
    }
}
