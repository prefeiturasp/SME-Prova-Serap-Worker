namespace SME.SERAp.Prova.Infra
{
    public class AlternativasProvaIdDto
    {
        public long Id  { get; set; }
        public long ItemId  { get; set; }
        public long ProvaId  { get; set; }
        public int OrdemProva  { get; set; }
        public int OrdemAlternativa  { get; set; }
        public string Descricao  { get; set; }
        public string Alternativa  { get; set; }

        public AlternativasProvaIdDto(long id, long itemId, long provaId, int ordemProva, int ordemAlternativa, string descricao, string alternativa)
        {
            Id = id;
            ItemId = itemId;
            ProvaId = provaId;
            OrdemProva = ordemProva;
            OrdemAlternativa = ordemAlternativa;
            Descricao = descricao;
            Alternativa = alternativa;
        }

        public AlternativasProvaIdDto()
        {
        }
    }
}
