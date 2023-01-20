using System;

namespace SME.SERAp.Prova.Dominio
{
    public class TurmaAlunoHistorico : EntidadeBase
    {
        public TurmaAlunoHistorico() { }

        public TurmaAlunoHistorico(long id, long matricula, long turmaId, int anoLetivo, long alunoId, DateTime dataMatricula, DateTime? dataSituacao)
        {
            Id = id;
            Matricula = matricula;
            TurmaId = turmaId;
            AnoLetivo = anoLetivo;
            AlunoId = alunoId;
            DataMatricula = dataMatricula;
            DataSituacao = dataSituacao;
            AtualizadoEm = AtualizadoEm = DateTime.Now;
        }

        public TurmaAlunoHistorico(long matricula, long turmaId, int anoLetivo, long alunoId, DateTime dataMatricula, DateTime? dataSituacao)
        {
            Matricula = matricula;
            TurmaId = turmaId;
            AnoLetivo = anoLetivo;
            AlunoId = alunoId;
            DataMatricula = dataMatricula;
            DataSituacao = dataSituacao;
            CriadoEm = AtualizadoEm = DateTime.Now;
        }

        public long Matricula { get; set; }
        public long TurmaId { get; set; }
        public int AnoLetivo { get; set; }
        public long AlunoId { get; set; }
        public DateTime DataMatricula { get; set; }
        public DateTime? DataSituacao { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime AtualizadoEm { get; set; }
    }
}
