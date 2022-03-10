using System;

namespace SME.SERAp.Prova.Dominio
{
    public class TurmaAlunoHistorico : EntidadeBase
    {
        public TurmaAlunoHistorico(long turmaId, int anoLetivo, long alunoId, DateTime dataMatricula, DateTime dataSituacao)
        {
            TurmaId = turmaId;
            AnoLetivo = anoLetivo;
            AlunoId = alunoId;
            DataMatricula = dataMatricula;
            DataSituacao = dataSituacao;
            CriadoEm = AtualizadoEm = DateTime.Now;
        }

        public long TurmaId { get; set; }
        public int AnoLetivo { get; set; }
        public long AlunoId { get; set; }
        public DateTime DataMatricula { get; set; }
        public DateTime DataSituacao { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime AtualizadoEm { get; set; }
    }
}
