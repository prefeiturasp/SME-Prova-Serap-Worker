using System;

namespace SME.SERAp.Prova.Dominio
{
    public class Aluno : EntidadeBase
    {
        public Aluno()
        {

        }

        public Aluno(string nome, long turmaId, long ra, int situacao)
        {
            Nome = nome;
            TurmaId = turmaId;
            RA = ra;
            Situacao = situacao;
        }
        public string Nome { get; set; }
        public long TurmaId { get; set; }
        public long RA { get; set; }
        public int Situacao { get; set; }
        public string Sexo { get; set; }
        public DateTime DataNascimento { get; set; }
        public string NomeSocial { get; set; }
    }
}
