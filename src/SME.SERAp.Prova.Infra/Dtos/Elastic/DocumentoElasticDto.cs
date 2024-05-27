using Nest;

namespace SME.SERAp.Prova.Infra
{
    public class DocumentoElasticDto
    {
        [Text(Name = "Id")]
        public string Id { get; set; }        
    }
}