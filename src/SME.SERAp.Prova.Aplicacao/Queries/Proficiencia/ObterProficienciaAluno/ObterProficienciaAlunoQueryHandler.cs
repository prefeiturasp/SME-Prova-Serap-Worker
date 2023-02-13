using MediatR;
using SME.SERAp.Prova.Dados;
using SME.SERAp.Prova.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterProficienciaAlunoQueryHandler : IRequestHandler<ObterProficienciaAlunoQuery, AlunoProvaProficiencia>
    {
        
        private readonly IRepositorioAlunoProvaProficiencia repositorioAlunoProvaProficiencia;

        public ObterProficienciaAlunoQueryHandler(IRepositorioAlunoProvaProficiencia repositorioAlunoProvaProficiencia)
        {
            this.repositorioAlunoProvaProficiencia = repositorioAlunoProvaProficiencia ?? throw new ArgumentException(nameof(repositorioAlunoProvaProficiencia));
        }
        public async Task<AlunoProvaProficiencia> Handle(ObterProficienciaAlunoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAlunoProvaProficiencia.ObterProficienciaAlunoAsync(request.ProvaId, request.AlunoId, request.Tipo, request.Origem);
        }
    }
}
