using System.Text.Json.Serialization;

namespace SME.SERAp.Prova.Infra
{
    public class ObterItensProvaTaiDto
    {
        [JsonPropertyName("ESTUDANTE")]
        public string Estudante { get; set; }
        [JsonPropertyName("proficiencia")]
        public string Proficiencia { get; set; }
        [JsonPropertyName("AnoEscolarEstudante")]
        public string AnoEscolarEstudante { get; set; }
        [JsonPropertyName("idItem")]
        public string IdItem { get; set; }
        [JsonPropertyName("parA")]
        public string ParA { get; set; }
        [JsonPropertyName("parB")]
        public string ParB { get; set; }
        [JsonPropertyName("parC")]
        public string ParC { get; set; }
        [JsonPropertyName("anoEscolarItem")]
        public string AnoEscolarItem { get; set; }
        [JsonPropertyName("habilidade")]
        public string Habilidade { get; set; }
        [JsonPropertyName("assunto")]
        public string Assunto { get; set; }
        [JsonPropertyName("subAssunto")]
        public string SubAssunto { get; set; }
        [JsonPropertyName("n.Ij")]
        public string NIj { get; set; }
        [JsonPropertyName("componente")]
        public string Componente { get; set; }
    }
}
