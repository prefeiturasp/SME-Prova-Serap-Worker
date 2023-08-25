using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterUltimaProficienciaAlunoPorDisciplinaIdQuery : IRequest<(decimal proficiencia, AlunoProvaProficienciaOrigem origem)>
    {
        public ObterUltimaProficienciaAlunoPorDisciplinaIdQuery(long alunoRa, long? disciplinaId, long turmaId,
            long ueId, string ano, long dreId, string ueCodigo)
        {
            AlunoRa = alunoRa;
            DisciplinaId = disciplinaId;
            TurmaId = turmaId;
            UeId = ueId;
            Ano = ano;
            DreId = dreId;
            UeCodigo = ueCodigo;
        }

        public long AlunoRa { get; }
        public long? DisciplinaId { get; }
        public long TurmaId { get; }
        public long UeId { get; }
        public string Ano { get; }
        public long DreId { get; }
        public string UeCodigo { get; }
    }
}
