using MediatR;
using SME.SERAp.Prova.Dominio;
using SME.SERAp.Prova.Infra;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterTurmaAlunoHistoricoEolPorAlunoRaQuery : IRequest<IEnumerable<TurmaEolDto>>
    {
        public ObterTurmaAlunoHistoricoEolPorAlunoRaQuery(long alunoRa)
        {
            AlunoRa = alunoRa;
        }

        public long AlunoRa { get; set; }
    }
}
