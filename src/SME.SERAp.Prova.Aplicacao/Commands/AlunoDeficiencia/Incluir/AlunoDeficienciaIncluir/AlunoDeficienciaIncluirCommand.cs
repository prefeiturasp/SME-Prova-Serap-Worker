using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class AlunoDeficienciaIncluirCommand : IRequest<long>
    {
        public AlunoDeficienciaIncluirCommand(AlunoDeficiencia alunoDeficiencia)
        {
            AlunoDeficiencia = alunoDeficiencia;
        }

        public AlunoDeficiencia AlunoDeficiencia { get; set; }
    }
}
