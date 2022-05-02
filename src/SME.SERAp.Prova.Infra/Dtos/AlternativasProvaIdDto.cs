namespace SME.SERAp.Prova.Infra
{
    public class AlternativasProvaIdDto : DtoBase
    {
        public long AlternativaLegadoId { get; set; }
        public int Ordem { get; set; }
        public string Numeracao { get; set; }

        public string Descricao { get; set; }


        public AlternativasProvaIdDto(long alternativaLegadoId, string numeracao, string descricao, int ordem)
        {
            AlternativaLegadoId = alternativaLegadoId;
            Numeracao = numeracao;
            Descricao = descricao;
            Ordem = ordem;
        }

        public AlternativasProvaIdDto()
        {
        }
    }
}