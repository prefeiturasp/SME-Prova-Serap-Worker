namespace SME.SERAp.Prova.Infra.Dtos
{
    public class TurmaSgpDto
    {
        public string Ano { get; set; }
        public int AnoLetivo { get; set; }
        public string Codigo { get; set; }
        public int TipoTurma { get; set; }
        public int ModalidadeCodigo { get; set; }
        public string NomeTurma { get; set; }
        public int TipoTurno { get; set; }        
        public long UeId { get; set; }
        public long TurmaId { get; set; }
    }
}
