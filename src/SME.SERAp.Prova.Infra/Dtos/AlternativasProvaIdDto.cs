namespace SME.SERAp.Prova.Infra
{
    public class AlternativasProvaIdDto
    {
        public long AlternativaLegadoId { get; set; }
        public int Ordem { get; set; }
        public string Alternativa { get; set; }

        public string Descricao { get; set; }


        public AlternativasProvaIdDto(long alternativaLegadoId, string alternativa, string descricao, int ordem)
        {
            AlternativaLegadoId = alternativaLegadoId;
            Alternativa = alternativa;
            Descricao = descricao;
            Ordem = ordem;
        }

        public AlternativasProvaIdDto()
        {
        }
    }
}