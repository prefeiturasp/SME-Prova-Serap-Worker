using MediatR;
using SME.SERAp.Prova.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SERAp.Prova.Aplicacao.Queries.ObterAlunosProvaProficienciaBoletimPorProvaId
{
    public class ObterAlunosProvaProficienciaBoletimPorProvaIdQuery : IRequest<IEnumerable<AlunoProvaProficienciaBoletimDto>>
    {
        public ObterAlunosProvaProficienciaBoletimPorProvaIdQuery(long provaId)
        {
            ProvaId = provaId;
        }

        public long ProvaId { get; set; }
    }
}
