namespace SME.SERAp.Prova.Infra
{
    public class AlternativasProvaIdDto
    {
        public long AlternativaLegadoId  { get; set; }
        public long ProvaLegadoId  { get; set; }
        public long QuestaoLegadoId  { get; set; }
        public int Ordem  { get; set; }
        public string Alternativa  { get; set; }
        public string Descricao  { get; set; }
        public bool Correta  { get; set; }


        public AlternativasProvaIdDto(long alternativaLegadoId, long provaLegadoId, long questaoLegadoId, int ordem, string alternativa, string descricao, bool correta)
        {
            AlternativaLegadoId = alternativaLegadoId;
            ProvaLegadoId = provaLegadoId;
            QuestaoLegadoId = questaoLegadoId;
            Ordem = ordem;
            Alternativa = alternativa;
            Descricao = descricao;
            Correta = correta;
        }

        public AlternativasProvaIdDto()
        {
        }
    }
}
