using MediatR;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TurmaAlunoHistoricoIncluirCommand : IRequest<long>
    {
        public TurmaAlunoHistoricoIncluirCommand(Dominio.TurmaAlunoHistorico turmaAlunoHistorico)
        {
            TurmaAlunoHistorico = turmaAlunoHistorico;
        }

        public Dominio.TurmaAlunoHistorico TurmaAlunoHistorico { get; set; }
    }
}
