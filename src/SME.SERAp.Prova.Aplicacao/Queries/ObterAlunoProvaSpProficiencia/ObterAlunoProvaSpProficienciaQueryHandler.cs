using MediatR;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Dominio.Entidades;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Prova.Aplicacao.Queries.ObterAlunoProvaSpProficiencia
{
    public class ObterAlunoProvaSpProficienciaQueryHandler : IRequestHandler<ObterAlunoProvaSpProficienciaQuery, AlunoProvaSpProficiencia>
    {
        private readonly IRepositorioAlunoProvaSpProficiencia repositorio;
        public ObterAlunoProvaSpProficienciaQueryHandler(IRepositorioAlunoProvaSpProficiencia repositorio)
        {
            this.repositorio = repositorio;
        }

        public Task<AlunoProvaSpProficiencia> Handle(ObterAlunoProvaSpProficienciaQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterAlunoProvaSpProficiencia(request.AnoLetivo, request.DisciplinaId, request.AlunoRa);
        }
    }
}
