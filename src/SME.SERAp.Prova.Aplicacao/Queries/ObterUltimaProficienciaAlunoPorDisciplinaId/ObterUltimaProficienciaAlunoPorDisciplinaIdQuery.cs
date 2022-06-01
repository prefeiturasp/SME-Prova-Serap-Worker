using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUltimaProficienciaAlunoPorDisciplinaIdQuery : IRequest<decimal>
    {
        public ObterUltimaProficienciaAlunoPorDisciplinaIdQuery(long alunoId, long? disciplinaId)
        {
            AlunoId = alunoId;
            DisciplinaId = disciplinaId;
        }

        public long AlunoId { get; set; }
        public long? DisciplinaId { get; set; }
    }
}
