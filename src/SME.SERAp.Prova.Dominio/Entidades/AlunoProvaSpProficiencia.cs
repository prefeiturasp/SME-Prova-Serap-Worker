using System;

namespace SME.SERAp.Prova.Dominio.Entidades
{
    public class AlunoProvaSpProficiencia : EntidadeBase
    {
        public long AlunoRa { get; set; }

        public int AnoEscolar { get; set; }

        public int AnoLetivo { get; set; }

        public long DisciplinaId { get; set; }

        public decimal Proficiencia { get; set; }
        public int NivelProficiencia { get; set; }

        public string UeCodigo { get; set; }

        public DateTime DataAtualizacao { get; set; }
    }
}
