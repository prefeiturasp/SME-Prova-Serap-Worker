using MediatR;
using SME.SERAp.Prova.Dominio;

namespace SME.SERAp.Prova.Aplicacao
{
    public class IncluirAlunoProvaProficienciaCommand : IRequest<long>
    {
        public IncluirAlunoProvaProficienciaCommand(AlunoProvaProficiencia alunoProvaProficiencia)
        {
            AlunoProvaProficiencia = alunoProvaProficiencia;
        }

        public AlunoProvaProficiencia AlunoProvaProficiencia { get; set; }
    }
}
