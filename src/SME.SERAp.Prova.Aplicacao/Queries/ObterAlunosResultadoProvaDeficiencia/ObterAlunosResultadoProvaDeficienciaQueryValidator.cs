using FluentValidation;

namespace SME.SERAp.Prova.Aplicacao.Queries.ObterAlunosResultadoProvaDeficiencia
{
    public class ObterAlunosResultadoProvaDeficienciaQueryValidator : AbstractValidator<ObterAlunosResultadoProvaDeficienciaQuery>
    {
        public ObterAlunosResultadoProvaDeficienciaQueryValidator()
        {
            RuleFor(x => x.ProvaId)
                .NotEmpty()
                .WithMessage("Código da prova é obrigatório");

            RuleFor(x => x.TurmasCodigos)
                .NotNull()
                .WithMessage("Os códigos das turmas devem ser informados.");
        }
    }
}
