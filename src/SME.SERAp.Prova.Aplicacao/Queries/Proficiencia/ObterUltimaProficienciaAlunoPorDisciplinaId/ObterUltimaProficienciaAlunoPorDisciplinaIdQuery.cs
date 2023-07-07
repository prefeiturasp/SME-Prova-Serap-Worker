using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUltimaProficienciaAlunoPorDisciplinaIdQuery : IRequest<(decimal proficiencia, AlunoProvaProficienciaOrigem origem)>
    {
        public ObterUltimaProficienciaAlunoPorDisciplinaIdQuery(long alunoRa, long? disciplinaId)
        {
            AlunoRa = alunoRa;
            DisciplinaId = disciplinaId;
        }

        public long AlunoRa { get; }
        public long? DisciplinaId { get; }
    }
}
