using MediatR;
using SME.SERAp.Prova.Dominio.Entidades;

namespace SME.SERAp.Prova.Aplicacao.Commands
{
    public class InserirAlunoProvaSpProficienciaCommand : IRequest<long>
    {
        public AlunoProvaSpProficiencia AlunoProvaSpProficiencia { get; set; }
        public InserirAlunoProvaSpProficienciaCommand(AlunoProvaSpProficiencia alunoProvaSpProficiencia)
        {
            AlunoProvaSpProficiencia = alunoProvaSpProficiencia;
        }
    }
}
