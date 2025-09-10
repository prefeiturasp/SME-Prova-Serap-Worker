using System;

namespace SME.SERAp.Prova.Dominio
{
    public class AlunoProvaProficiencia : EntidadeBase
    {
        public AlunoProvaProficiencia()
        {
        }

        public long AlunoId { get; set; }
        public long Ra { get; set; }
        public long ProvaId { get; set; }
        public long? DisciplinaId { get; set; }
        public decimal Proficiencia { get; set; }
        public AlunoProvaProficienciaOrigem Origem { get; set; }
        public AlunoProvaProficienciaTipo Tipo { get; set; }
        public DateTime UltimaAtualizacao { get; set; }
        public decimal ErroMedida { get; set; }

        public void Validar()
        {
            if (Proficiencia < 0)
                throw new Exception("A proficiência não pode ser negativa.");
        }
    }
}
