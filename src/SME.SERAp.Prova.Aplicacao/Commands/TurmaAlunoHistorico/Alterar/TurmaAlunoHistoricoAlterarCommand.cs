using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class TurmaAlunoHistoricoAlterarCommand : IRequest<long>
    {
        public TurmaAlunoHistoricoAlterarCommand(TurmaAlunoHistorico turmaAlunoHistorico)
        {
            TurmaAlunoHistorico = turmaAlunoHistorico;
        }

        public TurmaAlunoHistorico TurmaAlunoHistorico { get; set; }
    }
}
