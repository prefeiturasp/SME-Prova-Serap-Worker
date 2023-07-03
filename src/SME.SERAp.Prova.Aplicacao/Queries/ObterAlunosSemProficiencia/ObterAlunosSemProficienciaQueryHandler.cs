using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunosSemProficienciaQueryHandler : IRequestHandler<ObterAlunosSemProficienciaQuery, IEnumerable<AlunoProvaDto>>
    {
        private readonly IRepositorioAlunoProvaProficiencia repositorioAlunoProvaProficiencia;

        public ObterAlunosSemProficienciaQueryHandler(IRepositorioAlunoProvaProficiencia repositorioAlunoProvaProficiencia)
        {
            this.repositorioAlunoProvaProficiencia = repositorioAlunoProvaProficiencia ?? throw new ArgumentException(nameof(repositorioAlunoProvaProficiencia));
        }

        public async Task<IEnumerable<AlunoProvaDto>> Handle(ObterAlunosSemProficienciaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAlunoProvaProficiencia.ObterAlunosSemProficienciaAsync();
        }
    }
}
