namespace SME.SERAp.Prova.Dominio
{
    public class CadernoAluno : EntidadeBase
    {
        public long AlunoId { get; set; }
        public long ProvaId { get; set; }
        public string Caderno { get; set; }

        public CadernoAluno(long alunoId, long provaId, string caderno) : this()
        {
            AlunoId = alunoId;
            ProvaId = provaId;
            Caderno = caderno;
        }

        public CadernoAluno()
        {

        }
    }
}