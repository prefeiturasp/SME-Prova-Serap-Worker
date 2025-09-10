namespace SME.SERAp.Prova.Dominio.Entidades
{
    public class BoletimProvaAluno : EntidadeBase
    {
        public BoletimProvaAluno()
        {

        }

        public BoletimProvaAluno(long dreId, string codigoUe, string nomeUe, long provaId,
            string provaDescricao, int anoEscolar, string turma, long alunoRa, string alunoNome,
            string disciplina, long disciplinaId, ProvaStatus provaStatus, decimal proficiencia,
            decimal erroMedida, long nivelCodigo) : this()
        {
            DreId = dreId;
            CodigoUe = codigoUe;
            NomeUe = nomeUe;
            ProvaId = provaId;
            ProvaDescricao = provaDescricao;
            AnoEscolar = anoEscolar;
            Turma = turma;
            AlunoRa = alunoRa;
            AlunoNome = alunoNome;
            Disciplina = disciplina;
            DisciplinaId = disciplinaId;
            ProvaStatus = provaStatus;
            Proficiencia = proficiencia;
            ErroMedida = erroMedida;
            NivelCodigo = nivelCodigo;
        }

        public long DreId { get; set; }

        public string CodigoUe { get; set; }

        public string NomeUe { get; set; }

        public long ProvaId { get; set; }

        public string ProvaDescricao { get; set; }

        public int AnoEscolar { get; set; }

        public string Turma { get; set; }

        public long AlunoRa { get; set; }

        public string AlunoNome { get; set; }

        public string Disciplina { get; set; }

        public long DisciplinaId { get; set; }

        public ProvaStatus ProvaStatus { get; set; }

        public decimal Proficiencia { get; set; }

        public decimal ErroMedida { get; set; }

        public long NivelCodigo { get; set; }
    }
}
