using System;

namespace SME.SERAp.Prova.Dominio.Entidades.Presenca
{
    public class ProvaPresenca : EntidadeBase
    {
        public ProvaPresenca()
        {
            DataProcessamento = DateTime.UtcNow;
        }

        public string NomeProva { get; set; }
        public int AnoProva { get; set; }
        public string DescricaoProva { get; set; }
        public DateTime DataInicialAplicacao { get; set; }
        public DateTime DataFinalAplicacao { get; set; }
        public DateTime? DataCorte { get; set; }
        public bool VinculaAlunoCadernoExtra { get; set; }
        public DateTime DataProcessamento { get; set; }
    }
}