namespace SME.SERAp.Prova.Infra
{
    public class AlunoParaSincronizacaoInstitucionalDto : DtoBase
    {
        public AlunoParaSincronizacaoInstitucionalDto(long alunoCodigo, long turmaId)
        {
            AlunoCodigo = alunoCodigo;
            TurmaId = turmaId;
        }

        public long AlunoCodigo { get; }
        public long TurmaId { get; }
    }
}