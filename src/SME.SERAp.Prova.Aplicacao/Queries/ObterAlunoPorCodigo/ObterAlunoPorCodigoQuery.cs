using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunoPorCodigoQuery : IRequest<Aluno>
    {
        public ObterAlunoPorCodigoQuery(long codigoAluno)
        {
            CodigoAluno = codigoAluno;
        }

        public long CodigoAluno { get; set; }
    }
}
