using Nest;

namespace SME.SERAp.Prova.Infra
{
    [ElasticsearchType(RelationName = "Turma")]
    public class DocumentoElasticTurmaDto : DocumentoElasticDto
    {
        [Number(Name = "codigoturma")]
        public int CodigoTurma { get; set; }
        [Text(Name = "codigoescola")]
        public string CodigoEscola { get; set; }
        [Number(Name = "anoletivo")]
        public int AnoLetivo { get; set; }        
    }
}