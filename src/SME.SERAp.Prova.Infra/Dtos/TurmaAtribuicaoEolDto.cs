using System;

namespace SME.SERAp.Prova.Infra
{
    public class TurmaAtribuicaoEolDto
    {
        public int AnoLetivo { get; set; }
        public int DreCodigo { get; set; }
        public int UeCodigo { get; set; }
        public int TurmaCodigo { get; set; }
        public DateTime DataAtribuicao { get; set; }
        public DateTime? DataDisponibilizacaoAula { get; set; }
    }
}
