using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunosSemProficienciaPorProvaIdQueryHandler : IRequestHandler<ObterAlunosSemProficienciaPorProvaIdQuery, IEnumerable<AlunoProvaDto>>
    {
        private readonly IRepositorioAlunoProvaProficiencia repositorioAlunoProvaProficiencia;

        public ObterAlunosSemProficienciaPorProvaIdQueryHandler(IRepositorioAlunoProvaProficiencia repositorioAlunoProvaProficiencia)
        {
            this.repositorioAlunoProvaProficiencia = repositorioAlunoProvaProficiencia ?? throw new ArgumentNullException(nameof(repositorioAlunoProvaProficiencia));
        }

        public async Task<IEnumerable<AlunoProvaDto>> Handle(ObterAlunosSemProficienciaPorProvaIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAlunoProvaProficiencia.ObterAlunosSemProficienciaPorProvaIdAsync(request.ProvaId);
        }
    }
}