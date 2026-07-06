namespace SME.SERAp.Prova.Infra
{
    public class DocumentoElasticTurmaDto : DocumentoElasticDto
    {
        public int CodigoTurma { get; set; }
        public string CodigoEscola { get; set; }
        public int AnoLetivo { get; set; }
    }
}