using MediatR;
using SME.SERAp.Prova.Aplicacao.Queries.ObterResultadoAlunoProvaSp;
using SME.SERAp.Prova.Dados.Interfaces;
using SME.SERAp.Prova.Infra.Dtos;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Aplicacao.Queries
{
    public class ObterResultadoAlunoProvaSpQueryHandler : IRequestHandler<ObterResultadoAlunoProvaSpQuery, ResultadoAlunoProvaSpDto>
    {
        private readonly IRepositorioAlunoProvaSpProficiencia repositorio;
        public ObterResultadoAlunoProvaSpQueryHandler(IRepositorioAlunoProvaSpProficiencia repositorio)
        {
            this.repositorio = repositorio;
        }

        public Task<ResultadoAlunoProvaSpDto> Handle(ObterResultadoAlunoProvaSpQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterResultadoAlunoProvaSp(request.Edicao, request.AreaDoConhecimento, request.AlunoMatricula);
        }
    }
}
