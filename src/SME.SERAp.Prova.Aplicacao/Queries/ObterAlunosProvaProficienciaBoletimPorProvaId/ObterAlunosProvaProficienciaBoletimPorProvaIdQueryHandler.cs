using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.Queries.ObterAlunosProvaProficienciaBoletimPorProvaId
{
    public class ObterAlunosProvaProficienciaBoletimPorProvaIdQueryHandler : IRequestHandler<ObterAlunosProvaProficienciaBoletimPorProvaIdQuery, IEnumerable<AlunoProvaProficienciaBoletimDto>>
    {
        private readonly IRepositorioAlunoProvaProficiencia repositorioAlunoProvaProficiencia;

        public ObterAlunosProvaProficienciaBoletimPorProvaIdQueryHandler(IRepositorioAlunoProvaProficiencia repositorioAlunoProvaProficiencia)
        {
            this.repositorioAlunoProvaProficiencia = repositorioAlunoProvaProficiencia;
        }

        public Task<IEnumerable<AlunoProvaProficienciaBoletimDto>> Handle(ObterAlunosProvaProficienciaBoletimPorProvaIdQuery request, CancellationToken cancellationToken)
        {
            return repositorioAlunoProvaProficiencia.ObterAlunosProvaProficienciaBoletimPorProvaId(request.ProvaId);
        }
    }
}
