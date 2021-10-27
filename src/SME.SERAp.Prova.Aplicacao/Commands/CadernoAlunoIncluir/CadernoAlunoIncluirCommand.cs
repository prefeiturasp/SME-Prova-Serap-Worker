using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class CadernoAlunoIncluirCommand : IRequest<long>
    {
        public CadernoAlunoIncluirCommand(CadernoAluno cadernoAluno)
        {
            CadernoAluno = cadernoAluno;
        }

        public CadernoAluno CadernoAluno { get; set; }
    }
}
