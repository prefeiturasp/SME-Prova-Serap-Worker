namespace SME.SERAp.Prova.Infra
{
    public class TurmaSgpDto : DtoBase
    {
        public long Id { get; set; }
        public string Ano { get; set; }
        public int AnoLetivo { get; set; }
        public string Codigo { get; set; }
        public int TipoTurma { get; set; }
        public int ModalidadeCodigo { get; set; }
        public string NomeTurma { get; set; }
        public int TipoTurno { get; set; }        
        public long UeId { get; set; }
        public int Semestre { get; set; }
        public int EtapaEja { get; set; }
        public string SerieEnsino { get; set; }

        public bool DeveAtualizar(TurmaSgpDto turmaQuePodeAlterar)
        {
            return Ano != turmaQuePodeAlterar.Ano ||
                   AnoLetivo != turmaQuePodeAlterar.AnoLetivo ||
                   TipoTurma != turmaQuePodeAlterar.TipoTurma ||
                   ModalidadeCodigo != turmaQuePodeAlterar.ModalidadeCodigo ||
                   NomeTurma != turmaQuePodeAlterar.NomeTurma ||
                   TipoTurno != turmaQuePodeAlterar.TipoTurno ||
                   Semestre != turmaQuePodeAlterar.Semestre ||
                   EtapaEja != turmaQuePodeAlterar.EtapaEja ||
                   SerieEnsino != turmaQuePodeAlterar.SerieEnsino;     
        }
    }
}
