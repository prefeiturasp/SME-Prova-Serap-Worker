namespace SME.SERAp.Prova.Infra
{
    public class QuestoesPorProvaIdDto : DtoBase
    {
        public long QuestaoId  { get; set; }
        public int Ordem  { get; set; }
        public long ProvaLegadoId  { get; set; }
        public string TextoBase  { get; set; }
        public string Enunciado  { get; set; }
        public string Caderno { get; set; }
        public int TipoItem { get; set; }
        public int QuantidadeAlternativas { get; set; }

        public QuestoesPorProvaIdDto(long questaoId, int ordem, long provaLegadoId, string textoBase, string enunciado, int tipoItem, int quantidadeAlternativas, string caderno)
        {
            QuestaoId = questaoId;
            Ordem = ordem;
            ProvaLegadoId = provaLegadoId;
            TextoBase = textoBase;
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
