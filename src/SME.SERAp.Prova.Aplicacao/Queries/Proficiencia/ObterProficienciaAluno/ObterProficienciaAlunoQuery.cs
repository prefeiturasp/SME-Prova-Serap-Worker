using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterProficienciaAlunoQuery : IRequest<AlunoProvaProficiencia>
    {
        public ObterProficienciaAlunoQuery(long provaId, long alunoId, AlunoProvaProficienciaTipo tipo, AlunoProvaProficienciaOrigem origem)
        {
            ProvaId = provaId;
            AlunoId = alunoId;
            Tipo = tipo;
            Origem = origem;
        }

        public long ProvaId { get; set; }
        public long AlunoId { get; set; }
        public AlunoProvaProficienciaTipo Tipo { get; set; }
        public AlunoProvaProficienciaOrigem Origem { get; set; }

    }
}
