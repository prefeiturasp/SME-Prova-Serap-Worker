using System;
using Nest;

namespace SME.SERAp.Prova.Infra
{
    [ElasticsearchType(RelationName = "alunonaturma")]
    public class AlunoMatriculaTurmaDreDto : DocumentoElasticTurmaDto
    {
        [Number(Name = "codigoaluno")]
        public int CodigoAluno { get; set; }
        [Text(Name = "nomealuno")]
        public string NomeAluno { get; set; }
        [Date(Name = "datanascimento", Format = "MMddyyyy")]
        public DateTime DataNascimento { get; set; }
        [Text(Name = "nomesocialaluno")]
        public string NomeSocialAluno { get; set; }
        [Number(Name = "codigosituacaomatricula")]
        public int CodigoSituacaoMatricula { get; set; }
        [Text(Name = "situacaomatricula")]
        public string SituacaoMatricula { get; set; }
        [Date(Name = "datasituacao", Format = "MMddyyyy")]
        public DateTime DataSituacao { get; set; }
    }
}