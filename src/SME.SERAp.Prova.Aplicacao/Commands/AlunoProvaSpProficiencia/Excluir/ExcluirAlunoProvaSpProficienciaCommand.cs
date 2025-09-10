using MediatR;

namespace SME.SERAp.Prova.Aplicacao.Commands
{
    public class ExcluirAlunoProvaSpProficienciaCommand : IRequest<int>
    {
        public int AnoLetivo { get; set; }
        public long DisciplinaId { get; set; }
        public long AlunoRa { get; set; }
        public ExcluirAlunoProvaSpProficienciaCommand(int anoLetivo, long disciplinaId, long alunoRa)
        {
            AnoLetivo = anoLetivo;
            DisciplinaId = disciplinaId;
            AlunoRa = alunoRa;
        }
    }
}
