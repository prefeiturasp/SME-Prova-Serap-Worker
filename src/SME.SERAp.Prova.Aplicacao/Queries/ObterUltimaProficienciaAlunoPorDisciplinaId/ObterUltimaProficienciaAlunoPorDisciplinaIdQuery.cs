using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUltimaProficienciaAlunoPorDisciplinaIdQuery : IRequest<decimal>
    {
        public ObterUltimaProficienciaAlunoPorDisciplinaIdQuery(long alunoRa, long? disciplinaId)
        {
            AlunoRa = alunoRa;
            DisciplinaId = disciplinaId;
        }

        public long AlunoRa { get; set; }
        public long? DisciplinaId { get; set; }
    }
}
