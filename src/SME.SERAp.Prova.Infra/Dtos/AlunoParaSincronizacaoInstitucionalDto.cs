namespace SME.SERAp.Prova.Infra
{
    public class AlunoParaSincronizacaoInstitucionalDto : DtoBase
    {
        public AlunoParaSincronizacaoInstitucionalDto()
        {
        }

        public AlunoParaSincronizacaoInstitucionalDto(long id, long alunoCodigo, long turmaId)
        {
            Id = id;            
            AlunoCodigo = alunoCodigo;
            TurmaId = turmaId;
        }

        public long Id { get; }
        public long AlunoCodigo { get; }
        public long TurmaId { get; }
    }
}