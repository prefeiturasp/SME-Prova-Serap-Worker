using FluentValidation;

namespace SME.SERAp.Prova.Aplicacao
{
    public class ObterAlunosResultadoProvaAdesaoTodosQueryValidator : AbstractValidator<ObterAlunosResultadoProvaAdesaoTodosQuery>
    {
        public ObterAlunosResultadoProvaAdesaoTodosQueryValidator()
        {
            RuleFor(x => x.ProvaId)
                .NotEmpty()
                .WithMessage("Código da prova é obrigatório");
            
            RuleFor(x => x.TurmasCodigos)
                .NotEmpty()
                .WithMessage("Os códigos das turmas devem ser informados.");            
        }
    }
}