using System;

namespace SME.SERAp.Prova.Dominio
{
    public class Turma : EntidadeBase
    {
        public Turma()
        {
            DataAtualizacao = DateTime.Now;
        }
        public string Ano { get; set; }
        public int AnoLetivo { get; set; }
        public string Codigo { get; set; }
        public long UeId { get; set; }
        public int TipoTurma { get; set; }
        public int ModalidadeCodigo { get; set; }
        public string NomeTurma { get; set; }
        public int TipoTurno { get; set; }
        public DateTime DataAtualizacao { get; set; }
    }
}
