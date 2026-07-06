using System;

namespace SME.SERAp.Prova.Infra
{
    public class AlunoMatriculaTurmaDreDto : DocumentoElasticTurmaDto
    {
        public int CodigoAluno { get; set; }
        public string NomeAluno { get; set; }
        public DateTime DataNascimento { get; set; }
        public string NomeSocialAluno { get; set; }
        public int CodigoSituacaoMatricula { get; set; }
        public string SituacaoMatricula { get; set; }
        public DateTime DataSituacao { get; set; }
        public long CodigoMatricula { get; set; }
        public DateTime DataMatricula { get; set; }
    }
}