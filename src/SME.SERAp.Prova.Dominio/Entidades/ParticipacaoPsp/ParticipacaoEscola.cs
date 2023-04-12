using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SERAp.Prova.Dominio
{
    public class ParticipacaoEscola 
    {
        public string Edicao { get; set; }
        public string UadSigla { get; set; }
        public string EscCodigo { get; set; }
        public string AnoEscolar { get; set; }
        public int TotalPrevisto { get; set; }
        public int TotalPresente { get; set; }
        public decimal? PercentualParticipacao { get; set; }

    }
}
