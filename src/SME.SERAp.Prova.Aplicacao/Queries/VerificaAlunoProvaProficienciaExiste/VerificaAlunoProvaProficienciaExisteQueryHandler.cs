using MediatR;
using SME.SERAp.Prova.Dados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class VerificaAlunoProvaProficienciaExisteQueryHandler : IRequestHandler<VerificaAlunoProvaProficienciaExisteQuery, bool>
    {
        private readonly IRepositorioAlunoProvaProficiencia repositorioAlunoProvaProficiencia;

        public VerificaAlunoProvaProficienciaExisteQueryHandler(IRepositorioAlunoProvaProficiencia repositorioAlunoProvaProficiencia)
        {
            this.repositorioAlunoProvaProficiencia = repositorioAlunoProvaProficiencia ?? throw new ArgumentException(nameof(repositorioAlunoProvaProficiencia));
        }

        public async Task<bool> Handle(VerificaAlunoProvaProficienciaExisteQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAlunoProvaProficiencia.ExisteAsync(request.AlunoId, request.ProvaId);
        }
    }
}
